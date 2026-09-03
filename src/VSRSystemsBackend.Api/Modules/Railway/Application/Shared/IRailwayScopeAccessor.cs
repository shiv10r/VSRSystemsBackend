namespace VSRSystemsBackend.Api.Modules.Railway.Application.Shared;

public sealed record RailwayScope(
    Guid UserId,
    Guid OrganizationId,
    IReadOnlySet<Guid> DivisionIds,
    IReadOnlySet<string> Permissions)
{
    public void RequireDivision(Guid divisionId)
    {
        if (divisionId == Guid.Empty || !DivisionIds.Contains(divisionId))
            throw new UnauthorizedAccessException("The requested Railway division is outside the authenticated scope.");
    }

    public void RequirePermission(string permission)
    {
        if (!Permissions.Contains(permission))
            throw new UnauthorizedAccessException("The authenticated user does not have the required Railway permission.");
    }
}

public interface IRailwayScopeAccessor
{
    RailwayScope GetRequiredScope();
}
