using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VSRSystemsBackend.Api.Platform.Storage;

[ApiController]
[Authorize]
[Route("api/storage")]
public sealed class StorageController(SupabaseStorageService storage) : ControllerBase
{
    [HttpPost("uploads/sign")]
    public async Task<ActionResult<SignedUploadResponse>> SignUpload(
        [FromBody] SignedUploadRequest request,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(() => storage.CreateSignedUploadAsync(request, cancellationToken));
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
