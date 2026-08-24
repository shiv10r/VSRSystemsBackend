namespace VSRSystemsBackend.Api.Platform.Storage;

public sealed record SignedUploadRequest(string Bucket, string Path, string ContentType);

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

public sealed class StorageValidationException(string message) : Exception(message);

public sealed class StorageNotConfiguredException()
    : Exception("Supabase Storage is not configured.");

public sealed class StorageProviderException(string message, Exception? innerException = null)
    : Exception(message, innerException);
