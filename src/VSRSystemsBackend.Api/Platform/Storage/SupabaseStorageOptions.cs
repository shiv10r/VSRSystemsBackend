namespace VSRSystemsBackend.Api.Platform.Storage;

public sealed class SupabaseStorageOptions
{
    public const string SectionName = "SupabaseStorage";

    public string Url { get; set; } = string.Empty;
    public string ServiceRoleKey { get; set; } = string.Empty;
    public string[] AllowedBuckets { get; set; } = ["uploads"];
    public string[] AllowedContentTypes { get; set; } =
    [
        "image/jpeg",
        "image/png",
        "image/webp",
        "application/pdf",
        "text/plain",
        "text/csv"
    ];
    public int MaxPathLength { get; set; } = 512;
    public int SignedDownloadSeconds { get; set; } = 3_600;
}
