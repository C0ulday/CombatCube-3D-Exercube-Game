namespace Koboldgames.Primitives.Variables
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(DynamicVariableBase<,>), true)]
    internal class DynamicVariableEditor : PropertyDrawer
    {
        /// <summary>
        /// The GUI call of the serialized property.
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the property GUI.</param>
        /// <param name="property">The SerializedProperty to make the custom GUI for.</param>
        /// <param name="label">The label of this property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty sharedValue = property.FindPropertyRelative("sharedValue");

            label = EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, sharedValue, new GUIContent(property.displayName));

            if(EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }
    }
}
