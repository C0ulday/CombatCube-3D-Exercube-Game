namespace Koboldgames.NonNull.SettingsManagement
{
    using UnityEditor;
    using UnityEditor.SettingsManagement;

    internal class NonNullSetting<T> : UserSetting<T>
    {
        internal NonNullSetting(string key, T value, SettingsScope scope = SettingsScope.Project)
            : base(NonNullSettingsManager.Instance, key, value, scope) { }

        internal NonNullSetting(Settings settings, string key, T value, SettingsScope scope = SettingsScope.Project)
            : base(settings, key, value, scope) { }
    }
}
