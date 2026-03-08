namespace Koboldgames.Primitives
{
    using UnityEditor;
    using UnityEngine;

    internal abstract class SharedAssetEditor : Editor
    {
        protected SerializedProperty description;

        /// <summary>
        /// Inspector IMGUI call.
        /// </summary>
        public override void OnInspectorGUI()
        {
            description.stringValue = EditorGUILayout.TextField(
                description.displayName,
                description.stringValue,
                Styles.descriptionStyle,
                Styles.descriptionLayout
            );

            Editor.DrawPropertiesExcluding(serializedObject, "description", "m_Script");
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable() => description = serializedObject.FindProperty("description");

        /// <summary>
        /// Styles.
        /// </summary>
        internal static class Styles
        {
            public static readonly GUIStyle descriptionStyle = new GUIStyle(EditorStyles.textField);
            public static readonly GUILayoutOption[] descriptionLayout;

            static Styles()
            {
                descriptionStyle.wordWrap = true;
                descriptionStyle.stretchHeight = true;
                descriptionLayout = new GUILayoutOption[3] {
                    GUILayout.Height(48.0f),
                    GUILayout.ExpandHeight(true),
                    GUILayout.MaxHeight(64.0f),
                };
            }
        }
    }
}
