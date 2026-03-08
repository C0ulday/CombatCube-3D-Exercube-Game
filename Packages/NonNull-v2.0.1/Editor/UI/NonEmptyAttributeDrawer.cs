namespace Koboldgames.NonNull
{
    using System;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(NonEmptyAttribute))]
    internal class NonEmptyAttributeDrawer : BaseDrawer
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

            bool error;

            switch(property.propertyType)
            {
                case SerializedPropertyType.String:
                    error = String.IsNullOrEmpty(property.stringValue);
                    break;

                case SerializedPropertyType.AnimationCurve:
                    error = property.animationCurveValue == null;
                    error |= property.animationCurveValue.length == 0;
                    break;

                case SerializedPropertyType.LayerMask:
                    error = property.intValue == 0;
                    break;

                case SerializedPropertyType.ArraySize:
                    error = property.arraySize == 0;
                    break;

                case SerializedPropertyType.Color:
                    error = property.colorValue == null;
                    error |= property.colorValue == new Color(0f, 0f, 0f, 0f);
                    break;

                case SerializedPropertyType.Enum:
                    error = property.enumValueIndex == 0;
                    break;

                case SerializedPropertyType.Integer:
                    error = property.intValue == 0;
                    break;

                case SerializedPropertyType.Float:
                    error = Mathf.Approximately((float)property.doubleValue, 0f);
                    break;

                case SerializedPropertyType.Vector2:
                    error = property.vector2Value == Vector2.zero;
                    break;

                case SerializedPropertyType.Vector3:
                    error = property.vector3Value == Vector3.zero;
                    break;

                case SerializedPropertyType.Vector4:
                    error = property.vector4Value == Vector4.zero;
                    break;

                case SerializedPropertyType.Vector2Int:
                    error = property.vector2IntValue == Vector2Int.zero;
                    break;

                case SerializedPropertyType.Vector3Int:
                    error = property.vector3IntValue == Vector3Int.zero;
                    break;

                default:
                    Debug.LogWarning($"[NonNull] 'NonEmptyAttribute' on unsupported property type '{property.propertyType}'.");
                    error = false;
                    break;
            }

            if(error)
                NonNullPropertyField(position, property, label);
            else
                EditorGUI.PropertyField(position, property, label);

            EditorGUI.EndProperty();
        }
    }
}
