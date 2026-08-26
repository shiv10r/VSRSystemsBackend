using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;

namespace VSRSystemsBackend.Api.Modules.Railway.API.Controllers;

[ApiController]
[Authorize]
[Route("api/railway/offline-sync")]
public sealed class RailwayOfflineSyncController(
    IRailwayScopeAccessor scopeAccessor,
    RailwayOfflineSyncHandler handler) : ControllerBase
{
    [HttpPost(Name = "railway.offline-sync.execute")]
    public async Task<ActionResult<IReadOnlyList<RailwayOfflineCommandResult>>> Execute(
        IReadOnlyList<RailwayOfflineCommandEnvelope> commands,
        CancellationToken cancellationToken)
    {
        if (commands.Count is < 1 or > 100) return BadRequest();
        try
        {
            return Ok(await handler.HandleAsync(scopeAccessor.GetRequiredScope(), commands, cancellationToken));
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}
