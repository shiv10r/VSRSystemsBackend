using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Inspection;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

namespace VSRSystemsBackend.Api.Modules.Railway.API.Controllers;

[ApiController]
[Authorize]
[Route("api/railway/defects")]
public sealed class RailwayDefectsController(
    IRailwayScopeAccessor scopeAccessor,
    RailwayDbContext dbContext) : ControllerBase
{
    [HttpGet(Name = "railway.defects.list")]
    public async Task<ActionResult<IReadOnlyList<Defect>>> List(CancellationToken cancellationToken)
    {
        var scope = scopeAccessor.GetRequiredScope();
        scope.RequirePermission("railway.defects.read");
        return Ok(await dbContext.Defects.AsNoTracking()
            .Where(item => item.DivisionId.HasValue && scope.DivisionIds.Contains(item.DivisionId.Value))
            .OrderByDescending(item => item.RaisedAt).ToListAsync(cancellationToken));
    }

    [HttpPost("{defectId:guid}/triage", Name = "railway.defects.triage")]
    public async Task<ActionResult<Defect>> Triage(
        Guid defectId,
        [FromHeader(Name = "If-Match")] string ifMatch,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(ifMatch)) return BadRequest();
        var scope = scopeAccessor.GetRequiredScope();
        scope.RequirePermission("railway.defects.triage");
        var defect = await dbContext.Defects.SingleOrDefaultAsync(item => item.Id == defectId, cancellationToken);
        if (defect is null) return NotFound();
        scope.RequireDivision(defect.DivisionId!.Value);
        defect.Triage();
        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok(defect);
    }
}
