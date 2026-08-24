using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.Platform.Chat;
using VSRSystemsBackend.Shared.Constants;
using VSRSystemsBackend.Shared.DTOs;

namespace VSRSystemsBackend.Api.Platform.Chat;

[ApiController]
[Authorize]
[Route("api/v1/chat/conversations/{conversationId}/messages")]
public sealed class ChatController : ControllerBase
{
    private readonly IChatService _chat;

    public ChatController(IChatService chat)
    {
        _chat = chat;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<ChatMessagePageDto>>> GetMessages(
        string conversationId,
        [FromQuery] string? before = null,
        [FromQuery] int limit = 30,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized(ApiResponse<ChatMessagePageDto>.Fail("Authentication is required."));

        try
        {
            var result = await _chat.GetMessagesAsync(
                conversationId,
                userId,
                HasAdministrativeAccess(),
                before,
                limit,
                cancellationToken);
            return Ok(ApiResponse<ChatMessagePageDto>.Ok(result));
        }
        catch (ChatAccessDeniedException exception)
        {
            return StatusCode(StatusCodes.Status403Forbidden, ApiResponse<ChatMessagePageDto>.Fail(exception.Message));
        }
        catch (ChatValidationException exception)
        {
            return BadRequest(ApiResponse<ChatMessagePageDto>.Fail(exception.Message));
        }
        catch (ChatUnavailableException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ApiResponse<ChatMessagePageDto>.Fail(exception.Message));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ChatMessageDto>>> SendMessage(
        string conversationId,
        [FromBody] SendChatMessageRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized(ApiResponse<ChatMessageDto>.Fail("Authentication is required."));

        try
        {
            var result = await _chat.SendMessageAsync(
                conversationId,
                userId,
                HasAdministrativeAccess(),
                request,
                HttpContext.TraceIdentifier,
                cancellationToken);
            return StatusCode(StatusCodes.Status201Created, ApiResponse<ChatMessageDto>.Ok(result));
        }
        catch (ChatAccessDeniedException exception)
        {
            return StatusCode(StatusCodes.Status403Forbidden, ApiResponse<ChatMessageDto>.Fail(exception.Message));
        }
        catch (ChatValidationException exception)
        {
            return BadRequest(ApiResponse<ChatMessageDto>.Fail(exception.Message));
        }
        catch (ChatUnavailableException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ApiResponse<ChatMessageDto>.Fail(exception.Message));
        }
    }

    private string? GetUserId() =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? User.FindFirst(AppConstants.ClaimTypes.UserId)?.Value;

    private bool HasAdministrativeAccess() =>
        User.IsInRole("admin")
        || User.IsInRole("ops_agent")
        || User.IsInRole("support_agent");
}
