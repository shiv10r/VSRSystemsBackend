namespace VSRSystemsBackend.Api.Platform.Storage;

public sealed record SignedUploadRequest(
    string Bucket,
    string Path,
    string ContentType,
    bool BillingConfirmed = false);

public sealed record SignedUploadResponse(
    string Bucket,
    string Path,
    string ContentType,
    string SignedUrl,
    int ExpiresInSeconds);

public sealed record StorageObjectRequest(string Bucket, string Path);

public sealed record SignedDownloadResponse(
    string Bucket,
    string Path,
    string SignedUrl,
    int ExpiresInSeconds);

public sealed record DeleteObjectResponse(string Bucket, string Path, bool Deleted);

public sealed record UploadCompletedRequest(
    string Bucket,
    string Path,
    string FileName,
    string ContentType,
    long SizeBytes);

public sealed record UploadCompletedResponse(bool NotificationSent, string Message);

public sealed class StorageValidationException(string message) : Exception(message);

public sealed class StorageNotConfiguredException()
    : Exception("Supabase Storage is not configured.");

public sealed class StorageProviderException(string message, Exception? innerException = null)
    : Exception(message, innerException);
