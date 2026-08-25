using Microsoft.Extensions.Options;

namespace VSRSystemsBackend.Api.Platform.FeatureFlags;

public sealed class FeatureFlagService(IOptions<FeatureFlagsOptions> options)
{
    private readonly FeatureFlagsOptions _options = options.Value;

    public bool IsEnabled(string featureName) =>
        !string.IsNullOrWhiteSpace(featureName) && _options.Enabled;

    public bool IsModuleVisible(string moduleKey) =>
        !string.IsNullOrWhiteSpace(moduleKey) && _options.Enabled && _options.ModuleVisibility;

    public bool IsBrandingVisible() => _options.Enabled && _options.Branding;

    public bool IsCurrencyEnabled() => _options.Enabled && _options.Currency;

    public bool IsTimezoneEnabled() => _options.Enabled && _options.Timezone;
}
