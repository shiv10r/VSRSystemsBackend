public class FeatureFlagsOptions
{
    public const string SectionName = "FeatureFlags";

    public bool Enabled { get; set; } = true;
    public bool ModuleVisibility { get; set; } = true;
    public bool Branding { get; set; } = true;
    public bool Currency { get; set; } = true;
    public bool Timezone { get; set; } = true;
}