#if !UNITY_2018_3_OR_NEWER

namespace Koboldgames.NonNull.SettingsManagement
{
    using UnityEditor;
    using UnityEditor.SettingsManagement;

    public sealed class NonNullSettingsWindow : EditorWindow
    {
        private static NonNullSettingsWindow window;

        private UserSettingsProvider settingsProvider;

        /// <summary>
        /// Static window initialization method.
        /// </summary>
        [MenuItem("Edit/Project Settings/NonNull")]
        public static void Init()
        {
            window = GetWindow<NonNullSettingsWindow>(
                true,
                "NonNull Settings",
                true
            );
            window.Show();
        }

        /// <summary>
        /// Is called for rendering and handling GUI events in this editor window.
        /// </summary>
        private void OnGUI()
        {
            if(settingsProvider == null)
            {
                settingsProvider = new UserSettingsProvider(
                    NonNullSettingsManager.Instance,
                    new[] { typeof(UserSettingsProvider).Assembly }
                );
            }

            settingsProvider.OnGUI(null);
        }
    }
}

#endif
