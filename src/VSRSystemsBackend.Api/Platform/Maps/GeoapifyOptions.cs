namespace VSRSystemsBackend.Api.Platform.Maps;

public sealed class GeoapifyOptions
{
    public const string SectionName = "Geoapify";

    public string BaseUrl { get; set; } = "https://api.geoapify.com/";
    public string ApiKey { get; set; } = string.Empty;
    public int CacheHours { get; set; } = 24;
    public int MaxResults { get; set; } = 6;
    public int DailyProviderCallLimit { get; set; } = 2_700;
    public int UsageWarningCalls { get; set; } = 2_400;
    public int RequestsPerSecond { get; set; } = 5;
    public string CountryBias { get; set; } = "in";
}
