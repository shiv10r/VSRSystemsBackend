using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.Swagger;
using Xunit;

namespace VSRSystemsBackend.IntegrationTests.Modules.Railway;

public sealed class RailwayOpenApiExportTests
{
    [Fact]
    public void Export_writes_only_railway_paths_without_opening_a_port()
    {
        var outputPath = Environment.GetEnvironmentVariable("RAILWAY_OPENAPI_OUTPUT");
        if (string.IsNullOrWhiteSpace(outputPath))
            return;

        using var factory = new WebApplicationFactory<Program>();
        var document = factory.Services.GetRequiredService<ISwaggerProvider>().GetSwagger("v1");
        var nonRailwayPaths = document.Paths.Keys
            .Where(path => !path.StartsWith("/api/railway", StringComparison.OrdinalIgnoreCase))
            .ToArray();
        foreach (var path in nonRailwayPaths)
            document.Paths.Remove(path);

        var initialJson = Serialize(document);
        var root = JsonNode.Parse(initialJson)!;
        var requiredSchemas = new HashSet<string>(StringComparer.Ordinal);
        CollectSchemaReferences(root["paths"], requiredSchemas);
        var schemas = root["components"]?["schemas"] as JsonObject;
        var pending = new Queue<string>(requiredSchemas);
        var processed = new HashSet<string>(StringComparer.Ordinal);
        while (pending.TryDequeue(out var schemaName))
        {
            if (!processed.Add(schemaName))
                continue;
            CollectSchemaReferences(schemas?[schemaName], requiredSchemas);
            foreach (var discovered in requiredSchemas.Where(name => !processed.Contains(name)))
                    pending.Enqueue(discovered);
        }
        foreach (var schemaName in document.Components.Schemas.Keys.Where(name => !requiredSchemas.Contains(name)).ToArray())
            document.Components.Schemas.Remove(schemaName);

        var output = Serialize(document);

        var fullPath = Path.GetFullPath(outputPath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
        File.WriteAllText(fullPath, output + Environment.NewLine, Encoding.UTF8);

        Assert.NotEmpty(document.Paths);
        Assert.All(document.Paths.Keys, path => Assert.StartsWith("/api/railway", path));
    }

    private static string Serialize(Microsoft.OpenApi.Models.OpenApiDocument document)
    {
        var builder = new StringBuilder();
        var writer = new OpenApiJsonWriter(new StringWriter(builder));
        document.SerializeAsV3(writer);
        writer.Flush();
        return builder.ToString();
    }

    private static void CollectSchemaReferences(JsonNode? node, ISet<string> references)
    {
        if (node is JsonObject objectNode)
        {
            if (objectNode["$ref"]?.GetValue<string>() is { } reference)
                references.Add(reference[(reference.LastIndexOf('/') + 1)..]);
            foreach (var child in objectNode)
                CollectSchemaReferences(child.Value, references);
        }
        else if (node is JsonArray arrayNode)
        {
            foreach (var child in arrayNode)
                CollectSchemaReferences(child, references);
        }
    }
}
