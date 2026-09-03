using System.Security.Claims;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

public sealed class RailwayScopeAccessor(IHttpContextAccessor httpContextAccessor) : IRailwayScopeAccessor
{
    private static readonly string[] OrganizationClaimTypes = ["organization_id", "organizationId", "org_id"];
    private static readonly string[] DivisionClaimTypes = ["division_id", "division_ids", "divisionId", "divisionIds"];
    private static readonly string[] PermissionClaimTypes = ["permission", "permissions"];

    public RailwayScope GetRequiredScope()
    {
        var principal = httpContextAccessor.HttpContext?.User;
        if (principal?.Identity?.IsAuthenticated != true)
            throw new UnauthorizedAccessException("An authenticated Railway session is required.");

        var userId = ParseRequiredGuid(principal, [ClaimTypes.NameIdentifier, "sub", "user_id"], "user");
        var organizationId = ParseRequiredGuid(principal, OrganizationClaimTypes, "organization");
        var divisionIds = ReadValues(principal, DivisionClaimTypes)
            .Select(value => Guid.TryParse(value, out var id) ? id : Guid.Empty)
            .Where(id => id != Guid.Empty)
            .ToHashSet();
        var permissions = ReadValues(principal, PermissionClaimTypes)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        return new RailwayScope(userId, organizationId, divisionIds, permissions);
    }

    private static Guid ParseRequiredGuid(ClaimsPrincipal principal, IEnumerable<string> claimTypes, string label)
    {
        var value = claimTypes
            .Select(type => principal.FindFirst(type)?.Value)
            .FirstOrDefault(candidate => !string.IsNullOrWhiteSpace(candidate));

        if (!Guid.TryParse(value, out var id) || id == Guid.Empty)
            throw new UnauthorizedAccessException($"The authenticated session has no valid {label} claim.");

        return id;
    }

    private static IEnumerable<string> ReadValues(ClaimsPrincipal principal, IEnumerable<string> claimTypes) =>
        principal.Claims
            .Where(claim => claimTypes.Contains(claim.Type, StringComparer.OrdinalIgnoreCase))
            .SelectMany(claim => claim.Value.Split([',', ' ', ';'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
}
