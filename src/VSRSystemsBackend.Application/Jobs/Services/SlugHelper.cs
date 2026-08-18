namespace VSRSystemsBackend.Application.Jobs.Services;

/// <summary>
/// Generates URL-safe slugs used by Job and Company entities.
/// </summary>
public static class SlugHelper
{
    public static string GenerateSlug(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Guid.NewGuid().ToString("N")[..20];

        var slug = text.ToLowerInvariant().Trim();
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9]+", "-");
        slug = slug.Trim('-');
        return slug.Length > 200 ? slug[..200] : slug;
    }

    public static string EnsureUniqueSlug(string slug, Func<string, Task<bool>> existsAsync)
    {
        if (!existsAsync(slug).GetAwaiter().GetResult())
            return slug;

        var suffix = Guid.NewGuid().ToString("N")[..6];
        var candidate = slug;
        while (candidate.Length + 1 + suffix.Length > 200)
            candidate = candidate[..^1];

        return $"{candidate}-{suffix}";
    }

    public static async Task<string> EnsureUniqueSlugAsync(string slug, Func<string, Task<bool>> existsAsync)
    {
        if (!await existsAsync(slug))
            return slug;

        var suffix = Guid.NewGuid().ToString("N")[..6];
        var candidate = slug;
        while (candidate.Length + 1 + suffix.Length > 200)
            candidate = candidate[..^1];

        return $"{candidate}-{suffix}";
    }
}
