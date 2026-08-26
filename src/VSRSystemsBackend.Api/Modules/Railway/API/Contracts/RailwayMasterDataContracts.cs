namespace VSRSystemsBackend.Api.Modules.Railway.API.Contracts;

public sealed record CreateRailwayMasterRecordRequest(
    Guid? DivisionId,
    string Code,
    string Name,
    Guid? ParentId = null,
    Guid? SecondaryParentId = null,
    Guid? TertiaryParentId = null,
    DateTimeOffset? EffectiveFrom = null,
    TimeOnly? DepartureWindowStart = null,
    TimeOnly? DepartureWindowEnd = null,
    string? Status = null,
    string? Criticality = null,
    double? Latitude = null,
    double? Longitude = null,
    IReadOnlyList<RailwayCoordinate>? Geometry = null);

public sealed record UpdateRailwayMasterRecordRequest(string Code, string Name);

public sealed record RailwayCoordinate(double Latitude, double Longitude);
