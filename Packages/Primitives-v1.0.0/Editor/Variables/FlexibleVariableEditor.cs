namespace Koboldgames.Primitives.Variables
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(FlexibleVariableBase<,>), true)]
    internal class FlexibleVariableEditor : PropertyDrawer
    {
        /// <summary>
        /// The GUI call of the serialized property.
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the property GUI.</param>
        /// <param name="property">The SerializedProperty to make the custom GUI for.</param>
        /// <param name="label">The label of this property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginChangeCheck();

            SerializedProperty useLocal = property.FindPropertyRelative("useLocal");
            SerializedProperty localValue = property.FindPropertyRelative("localValue");
            SerializedProperty sharedValue = property.FindPropertyRelative("sharedValue");

            Rect buttonRect = new Rect(position);
            buttonRect.yMin += Styles.popupStyle.margin.top + 1f;
            buttonRect.yMax -= 1f;
            buttonRect.width = Styles.popupStyle.fixedWidth + Styles.popupStyle.margin.right;
            position.xMin = buttonRect.xMax;

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            int result = EditorGUI.Popup(
                buttonRect,
                useLocal.boolValue ? 0 : 1,
                Styles.popupOptions,
                Styles.popupStyle
            );

            useLocal.boolValue = (result == 0);

            if(useLocal.boolValue)
            {
                switch(localValue.propertyType)
                {
                    case SerializedPropertyType.Quaternion:
                        float[] qValues = {
                            localValue.quaternionValue.x,
                            localValue.quaternionValue.y,
                            localValue.quaternionValue.z,
                            localValue.quaternionValue.w,
                        };
                        EditorGUI.MultiFloatField(position, Styles.largeVectorLabels, qValues);
                        localValue.quaternionValue = new Quaternion(
                            qValues[0],
                            qValues[1],
                            qValues[2],
                            qValues[3]
                        );
                        break;

                    case SerializedPropertyType.Vector4:
                        float[] vValues = {
                            localValue.vector4Value.x,
                            localValue.vector4Value.y,
                            localValue.vector4Value.z,
                            localValue.vector4Value.w
                        };
                        EditorGUI.MultiFloatField(position, Styles.largeVectorLabels, vValues);
                        localValue.vector4Value = new Vector4(
                            vValues[0],
                            vValues[1],
                            vValues[2],
                            vValues[3]
                        );
                        break;

                    default:
                        EditorGUI.PropertyField(position, localValue, GUIContent.none);
                        break;
                }
            }
            else if(sharedValue != null)
            {
                EditorGUI.PropertyField(position, sharedValue, GUIContent.none);
            }

            if(EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Styles.
        /// </summary>
        internal static class Styles
        {
            public static readonly GUIStyle popupStyle = new GUIStyle("PaneOptions");
            public static readonly string[] popupOptions;
            public static readonly GUIContent[] largeVectorLabels;

            static Styles()
            {
                popupStyle.imagePosition = ImagePosition.ImageOnly;
                popupOptions = new string[] {
                    "Local Variable",
                    "Shared Variable"
                };
                largeVectorLabels = new GUIContent[] {
                    new GUIContent("X"),
                    new GUIContent("Y"),
                    new GUIContent("Z"),
                    new GUIContent("W"),
                };
            }
        }
    }
}
