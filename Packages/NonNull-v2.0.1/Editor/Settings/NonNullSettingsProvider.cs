#if UNITY_2018_3_OR_NEWER

namespace Koboldgames.NonNull.SettingsManagement
{
    using UnityEditor;
    using UnityEditor.SettingsManagement;

    internal static class NonNullSettingsProvider
    {
        const string PreferencesPath = "Project/Non Null";

        /// <summary>
        /// Create the non-null settings provider.
        /// </summary>
        /// <returns>The created settings provider.</returns>
        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            var provider = new UserSettingsProvider(
                PreferencesPath,
                NonNullSettingsManager.Instance,
                new[] { typeof(NonNullSettingsProvider).Assembly },
                SettingsScope.Project
            );

            return provider;
        }
    }
}

#endif
