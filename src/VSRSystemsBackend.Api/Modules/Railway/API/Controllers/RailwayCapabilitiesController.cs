using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;

namespace VSRSystemsBackend.Api.Modules.Railway.API.Controllers;

[ApiController]
[Authorize]
[Route("api/railway/capabilities")]
public sealed class RailwayCapabilitiesController(
    IRailwayScopeAccessor scopeAccessor,
    IRailwayFeatureGate featureGate) : ControllerBase
{
    [HttpGet(Name = "railway.capabilities.get")]
    [ProducesResponseType<RailwayCapabilities>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<RailwayCapabilities>> Get(CancellationToken cancellationToken)
    {
        RailwayScope scope;
        try
        {
            scope = scopeAccessor.GetRequiredScope();
        }
        catch (UnauthorizedAccessException exception)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Railway scope unavailable",
                detail: exception.Message);
        }

        return Ok(await featureGate.GetAsync(scope, cancellationToken));
    }
}
