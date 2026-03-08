namespace Koboldgames.NonNull.SettingsManagement
{
    using UnityEditor;
    using UnityEditor.SettingsManagement;

    internal static class NonNullSettingsManager
    {
        internal const string PackageName = "com.koboldgames.nonnull";

        private static Settings SettingsInstance;

        /// <summary>
        /// Gets the static settings manager instance.
        /// </summary>
        /// <value>The static settings manager instance.</value>
        internal static Settings Instance
        {
            get
            {
                if(SettingsInstance == null)
                    SettingsInstance = new Settings(PackageName);

                return SettingsInstance;
            }
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public static void Save() => Instance.Save();

        /// <summary>
        /// Get the value of a specific setting.
        /// </summary>
        /// <typeparam name="T">The type of the setting.</typeparam>
        /// <param name="key">The setting key.</param>
        /// <param name="scope">The scope of the setting.</param>
        /// <param name="fallback">The fallback default value of the setting.</param>
        /// <returns>The value of the specified setting.</returns>
        public static T Get<T>(string key, SettingsScope scope = SettingsScope.Project, T fallback = default(T)) =>
            Instance.Get<T>(key, scope, fallback);

        /// <summary>
        /// Set the value of a specific setting.
        /// </summary>
        /// <typeparam name="T">The type of the setting.</typeparam>
        /// <param name="key">The setting key.</param>
        /// <param name="value">The setting value.</param>
        /// <param name="scope">The scope of the setting.</param>
        public static void Set<T>(string key, T value, SettingsScope scope = SettingsScope.Project) =>
            Instance.Set<T>(key, value, scope);

        /// <summary>
        /// Checks if a specific setting key exists.
        /// </summary>
        /// <typeparam name="T">The type of the setting.</typeparam>
        /// <param name="key">The setting key.</param>
        /// <param name="scope">The scope of the setting.</param>
        /// <returns><c>true</c> if the specified key exists; otherwise <c>false</c>.</returns>
        public static bool ContainsKey<T>(string key, SettingsScope scope = SettingsScope.Project) =>
            Instance.ContainsKey<T>(key, scope);
    }
}
