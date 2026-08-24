namespace VSRSystemsBackend.Api.Platform.Weather;

public sealed class WeatherOptions
{
    public const string SectionName = "Weather";

    public string BaseUrl { get; set; } = "https://api.open-meteo.com/";
    public int CacheMinutes { get; set; } = 15;
    public int ForecastDays { get; set; } = 7;
}
