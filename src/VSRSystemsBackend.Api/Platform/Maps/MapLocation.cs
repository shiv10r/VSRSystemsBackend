namespace VSRSystemsBackend.Api.Platform.Maps;

public sealed record MapLocation(
    string Id,
    string Label,
    double Latitude,
    double Longitude,
    string Provider = "geoapify");
