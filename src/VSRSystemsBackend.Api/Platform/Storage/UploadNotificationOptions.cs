namespace VSRSystemsBackend.Api.Platform.Storage;

public sealed class UploadNotificationOptions
{
    public const string SectionName = "UploadNotifications";

    public string ResendApiKey { get; set; } = string.Empty;
    public string FromEmail { get; set; } = "VSR Systems <onboarding@resend.dev>";
    public string RecipientEmail { get; set; } = string.Empty;
}
