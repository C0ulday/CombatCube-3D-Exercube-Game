namespace Koboldgames.NonNull
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(AllowNullAttribute))]
    internal class AllowNullAttributeDrawer : PropertyDrawer
    {
        /// <summary>
        /// The GUI call of the serialized property.
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the property GUI.</param>
        /// <param name="property">The SerializedProperty to make the custom GUI for.</param>
        /// <param name="label">The label of this property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property, label);
            EditorGUI.EndProperty();
        }
    }
}
