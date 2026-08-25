public class SettingsOptions
{
    public const string SectionName = "Settings";

    public string BrandingName { get; set; } = "VSR Systems";
    public string Currency { get; set; } = "USD";
    public string Timezone { get; set; } = "UTC";
    public bool FeatureFlagsEnabled { get; set; } = true;
}