using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using VSRSystemsBackend.Api.Controllers;
using VSRSystemsBackend.Api.Infrastructure.Authentication;
using Xunit;

namespace VSRSystemsBackend.IntegrationTests;

public sealed class AuthControllerHttpTests
{
    [Fact]
    public async Task Hard_coded_admin_credentials_work_over_http()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddControllers().AddApplicationPart(typeof(AuthController).Assembly);
        builder.Services.AddAuthentication(CacheTokenAuthenticationHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, CacheTokenAuthenticationHandler>(
                CacheTokenAuthenticationHandler.SchemeName,
                _ => { });
        builder.Services.AddAuthorization();

        await using var app = builder.Build();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        await app.StartAsync();

        using var client = app.GetTestClient();
        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            email = "admin.portal@vsrsystems.com",
            password = "nfeuTYjb7CEAnoK7EV"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Admin", body.GetProperty("username").GetString());
        Assert.Equal("admin", body.GetProperty("role").GetString());
        var token = body.GetProperty("token").GetString();
        Assert.Equal(32, token!.Length);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var currentUser = await client.GetAsync("/api/auth/me");
        Assert.Equal(HttpStatusCode.OK, currentUser.StatusCode);

        var rejected = await client.PostAsJsonAsync("/api/auth/login", new
        {
            email = "admin.portal@vsrsystems.com",
            password = "wrong-password"
        });
        Assert.Equal(HttpStatusCode.Unauthorized, rejected.StatusCode);
    }
}
