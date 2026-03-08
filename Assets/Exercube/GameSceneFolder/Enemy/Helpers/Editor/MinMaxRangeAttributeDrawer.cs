using UnityEngine;
using UnityEditor;
using Sphery.ExerCube.Helpers;

namespace Sphery.ExerCubeEditor.Helpers
{
    [CustomPropertyDrawer(typeof(MinMaxRange))]
    [CustomPropertyDrawer(typeof(MinMaxIntRange))]
    [CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
    public class MinMaxRangeAttributeDrawer : PropertyDrawer
    {
        /// <summary>
        /// Is called for rendering and handling GUI events for this property.
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the property GUI.</param>
        /// <param name="property">The <see cref="SerializedProperty"/> to make the custom GUI for.</param>
        /// <param name="label">The label of this property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Local property variables
            SerializedProperty minProp = property.FindPropertyRelative("min");
            SerializedProperty maxProp = property.FindPropertyRelative("max");
            MinMaxRangeAttribute attr = attribute as MinMaxRangeAttribute;
            bool isInteger = (attr?.Integer == true) || (property.type == "MinMaxIntRange");
            float selectedMin = isInteger ? minProp.intValue : minProp.floatValue;
            float selectedMax = isInteger ? maxProp.intValue : maxProp.floatValue;
            float boundaryMin = attr?.Min ?? 0f;
            float boundaryMax = attr?.Max ?? (isInteger ? 10f : 1f);

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            // Don't make child fields be indented
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect minField = position;
            Rect maxField = position;

            minField.xMax = minField.xMin + 50f;
            maxField.xMin = maxField.xMax - 50f;
            position.xMin += 55f;
            position.xMax -= 55f;

            // Check for changes
            EditorGUI.BeginChangeCheck();

            selectedMin = isInteger ?
                          EditorGUI.IntField(minField, Mathf.RoundToInt(selectedMin)) :
                          EditorGUI.FloatField(minField, selectedMin);

            EditorGUI.MinMaxSlider(position, ref selectedMin, ref selectedMax, boundaryMin, boundaryMax);

            selectedMax = isInteger ?
                          EditorGUI.IntField(maxField, Mathf.RoundToInt(selectedMax)) :
                          EditorGUI.FloatField(maxField, selectedMax);

            // Check values
            if(selectedMin > selectedMax)
                selectedMin = selectedMax;

            selectedMin = Mathf.Clamp(selectedMin, boundaryMin, boundaryMax);
            selectedMax = Mathf.Clamp(selectedMax, boundaryMin, boundaryMax);

            // Values changed
            if(EditorGUI.EndChangeCheck())
            {
                if(isInteger)
                {
                    minProp.intValue = Mathf.RoundToInt(selectedMin);
                    maxProp.intValue = Mathf.RoundToInt(selectedMax);
                }
                else
                {
                    minProp.floatValue = selectedMin;
                    maxProp.floatValue = selectedMax;
                }

                property.serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}
