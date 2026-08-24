using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace VSRSystemsBackend.Api.Platform.Storage;

[ApiController]
[Authorize]
[Route("api/storage")]
public sealed class StorageController(
    SupabaseStorageService storage,
    UploadNotificationService notifications) : ControllerBase
{
    [HttpPost("uploads/sign")]
    public async Task<ActionResult<SignedUploadResponse>> SignUpload(
        [FromBody] SignedUploadRequest request,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(() => storage.CreateSignedUploadAsync(request, cancellationToken));
    }

    [HttpPost("uploads/completed")]
    public async Task<ActionResult<UploadCompletedResponse>> UploadCompleted(
        [FromBody] UploadCompletedRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.FileName) || request.FileName.Length > 255 || request.SizeBytes <= 0 || request.SizeBytes > 25 * 1024 * 1024)
            return BadRequest(new { error = "Upload details are invalid." });

        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
            return Ok(new UploadCompletedResponse(false, "Upload completed, but the account has no email address."));

        try
        {
            storage.Validate(new SignedUploadRequest(request.Bucket, request.Path, request.ContentType, true));
            await storage.VerifyObjectExistsAsync(new StorageObjectRequest(request.Bucket, request.Path), cancellationToken);
            var result = await notifications.SendAsync(email, request, cancellationToken);
            return Ok(new UploadCompletedResponse(result.Sent, result.Message));
        }
        catch (StorageValidationException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
        catch (StorageNotConfiguredException exception)
        {
            return Problem(exception.Message, statusCode: StatusCodes.Status503ServiceUnavailable);
        }
        catch (StorageProviderException exception)
        {
            return Problem(exception.Message, statusCode: StatusCodes.Status502BadGateway);
        }
    }

    [HttpPost("downloads/sign")]
    public async Task<ActionResult<SignedDownloadResponse>> SignDownload(
        [FromBody] StorageObjectRequest request,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(() => storage.CreateSignedDownloadAsync(request, cancellationToken));
    }

    [HttpDelete("objects")]
    public async Task<ActionResult<DeleteObjectResponse>> DeleteObject(
        [FromBody] StorageObjectRequest request,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(() => storage.DeleteObjectAsync(request, cancellationToken));
    }

    private async Task<ActionResult<T>> ExecuteAsync<T>(Func<Task<T>> operation)
    {
        try
        {
            return Ok(await operation());
        }
        catch (StorageValidationException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
        catch (StorageNotConfiguredException exception)
        {
            return Problem(exception.Message, statusCode: StatusCodes.Status503ServiceUnavailable);
        }
        catch (StorageProviderException exception)
        {
            return Problem(exception.Message, statusCode: StatusCodes.Status502BadGateway);
        }
    }
}
