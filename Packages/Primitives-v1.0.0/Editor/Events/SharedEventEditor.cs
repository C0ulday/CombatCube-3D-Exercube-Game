namespace Koboldgames.Primitives.Events
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(SharedEventBase), true)]
    internal class SharedEventGeneralEditor : SharedAssetEditor { }

    [CustomEditor(typeof(SharedEvent))]
    internal class SharedEventEditor : SharedAssetEditor
    {
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

            if(Application.isPlaying && GUILayout.Button("Manually Invoke"))
                (target as SharedEvent)?.Invoke();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
