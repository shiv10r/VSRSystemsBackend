namespace VSRSystemsBackend.Api.Platform.Maps;

public sealed class MapsNotConfiguredException : InvalidOperationException
{
    public MapsNotConfiguredException() : base("Geoapify is not configured.") { }
}

public sealed class MapsQuotaExceededException : InvalidOperationException
{
    public MapsQuotaExceededException() : base("The daily Geoapify provider-call limit has been reached.") { }
}

public sealed class MapsProviderException : Exception
{
    public MapsProviderException(string message) : base(message) { }
    public MapsProviderException(string message, Exception innerException) : base(message, innerException) { }
}
