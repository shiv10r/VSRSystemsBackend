using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

namespace VSRSystemsBackend.Api.Modules.Railway.API.Controllers;
[ApiController, Authorize, Route("api/railway/audit")]
public sealed class RailwayAuditController(IRailwayScopeAccessor scopeAccessor, RailwayDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? resourceType, CancellationToken token)
    {
        var scope = scopeAccessor.GetRequiredScope(); scope.RequirePermission("railway.audit.read");
        var query = db.AuditRecords.AsNoTracking().Where(item => item.DivisionId == null || scope.DivisionIds.Contains(item.DivisionId.Value));
        if (!string.IsNullOrWhiteSpace(resourceType)) query = query.Where(item => item.ResourceType == resourceType);
        return Ok(await query.OrderByDescending(item => item.OccurredAt).Take(500).ToArrayAsync(token));
    }
}
