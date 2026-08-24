namespace VSRSystemsBackend.Api.Platform.Maps;

public sealed class OlaMapsOptions
{
    public const string SectionName = "OlaMaps";

    public string BaseUrl { get; set; } = "https://api.olamaps.io/";
    public string ApiKey { get; set; } = string.Empty;
    public int CacheHours { get; set; } = 24;
    public int MaxResults { get; set; } = 6;
    public int MonthlyProviderCallLimit { get; set; } = 90_000;
    public int UsageWarningPercent { get; set; } = 80;
}
