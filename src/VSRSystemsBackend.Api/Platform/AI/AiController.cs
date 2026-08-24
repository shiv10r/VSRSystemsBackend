using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VSRSystemsBackend.Api.Platform.AI;

[ApiController]
[Authorize]
public sealed class AiController(AiGatewayService gateway) : ControllerBase
{
    [HttpGet("/api/ai/status")]
    [HttpGet("/api/assistant/ai/status")]
    public ActionResult<AiStatus> Status() => Ok(gateway.GetStatus());

    [HttpPost("/api/ai/chat")]
    [HttpPost("/api/assistant/ai")]
    public async Task<ActionResult<AiReply>> Chat(
        [FromBody] AiChatRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Text) || request.Text.Length > 4_000)
            return BadRequest(new { error = "Text must contain between 1 and 4000 characters." });

        var history = request.History ?? [];
        if (history.Count > 40
            || history.Any(turn => turn is null
                || string.IsNullOrEmpty(turn.Content)
                || turn.Content.Length > 4_000
                || turn.Role is not ("user" or "assistant" or "system")))
            return BadRequest(new { error = "History contains an invalid role, size, or message." });

        try
        {
            return Ok(await gateway.ChatAsync(request.Text.Trim(), history, cancellationToken));
        }
        catch (AiProviderRejectedException exception)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new AiReply(
                false,
                true,
                exception.Model,
                string.Empty,
                0,
                exception.Message));
        }
    }
}
