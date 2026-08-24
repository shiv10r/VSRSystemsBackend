namespace VSRSystemsBackend.Api.Platform.Weather;

public sealed record DailyForecast(
    DateOnly Date,
    int WeatherCode,
    double TempMax,
    double TempMin,
    int RainProbability);

public sealed record ProjectWeather(
    double Temperature,
    double FeelsLike,
    int Humidity,
    double WindSpeed,
    int RainProbability,
    double Precipitation,
    int WeatherCode,
    bool IsDay,
    string Condition,
    IReadOnlyList<DailyForecast> Forecast,
    DateTimeOffset UpdatedAt);

public sealed class WeatherProviderException : Exception
{
    public WeatherProviderException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
