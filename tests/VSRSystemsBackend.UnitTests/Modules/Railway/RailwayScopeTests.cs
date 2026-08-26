using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;
using Xunit;

namespace VSRSystemsBackend.UnitTests.Modules.Railway;

public sealed class RailwayScopeTests
{
    [Fact]
    public void GetRequiredScope_rejects_a_user_without_an_organization_claim()
    {
        var accessor = CreateAccessor(
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));

        Assert.Throws<UnauthorizedAccessException>(() => accessor.GetRequiredScope());
    }

    [Fact]
    public void GetRequiredScope_reads_divisions_and_permissions_from_claims()
    {
        var userId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        var divisionA = Guid.NewGuid();
        var divisionB = Guid.NewGuid();
        var accessor = CreateAccessor(
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim("organization_id", organizationId.ToString()),
            new Claim("division_ids", $"{divisionA},{divisionB}"),
            new Claim("permissions", "railway.inspections.read railway.defects.read"));

        var scope = accessor.GetRequiredScope();

        Assert.Equal(userId, scope.UserId);
        Assert.Equal(organizationId, scope.OrganizationId);
        Assert.Equal(new HashSet<Guid> { divisionA, divisionB }, scope.DivisionIds);
        Assert.Contains("railway.inspections.read", scope.Permissions);
    }

    [Fact]
    public void Railway_entities_require_organization_ownership()
    {
        Assert.Throws<ArgumentException>(() => new TestRailwayEntity(Guid.NewGuid(), Guid.Empty));
    }

    private static RailwayScopeAccessor CreateAccessor(params Claim[] claims)
    {
        var context = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test")),
        };
        return new RailwayScopeAccessor(new HttpContextAccessor { HttpContext = context });
    }

    private sealed class TestRailwayEntity(Guid id, Guid organizationId)
        : RailwayEntity(id, organizationId);
}
