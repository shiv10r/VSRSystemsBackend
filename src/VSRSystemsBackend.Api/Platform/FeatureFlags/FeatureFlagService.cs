using Microsoft.Extensions.Options;
using VSRSystemsBackend.Api.Platform.FeatureFlags;
using VSRSystemsBackend.Api.Platform.Settings;

namespace VSRSystemsBackend.Api.Platform
{
    public class FeatureFlagService
    {
        private readonly FeatureFlagsOptions _options;

        public FeatureFlagService(IOptions<FeatureFlagsOptions> options)
        {
            _options = options.Value;
        }

        public bool IsEnabled(string featureName)
        {
            // In a full implementation, this would check per-organization or per-feature settings
            // For now, respect the global enabled flag
            return _options.Enabled;
        }

        public bool IsModuleVisible(string moduleKey)
        {
            return _options.ModuleVisibility;
        }

        public bool IsBrandingVisible()
        {
            return _options.Branding;
        }

        public bool IsCurrencyEnabled()
        {
            return _options.Currency;
        }

        public bool IsTimezoneEnabled()
        {
            return _options.Timezone;
        }
    }
}