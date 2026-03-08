using Sphery.ExerCube;
using UnityEditor;

namespace SpheryEditor.ExerCube
{
    [CustomEditor(typeof(Display))]
    public class DisplayEditor : Editor
    {
        private static readonly string[] targetDisplayOptions = new string[]
        {
        "Display 1",
        "Display 2",
        "Display 3",
        "Display 4",
        "Display 5",
        "Display 6",
        "Display 7",
        "Display 8"
        };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Display Settings", EditorStyles.boldLabel);

            SerializedProperty displayName = serializedObject.FindProperty("displayName");
            EditorGUILayout.PropertyField(displayName);

            EditorGUILayout.Space();

            // Draw window settings
            EditorGUILayout.LabelField("Camera Settings", EditorStyles.boldLabel);

            SerializedProperty targetDisplay = serializedObject.FindProperty("targetDisplay");
            targetDisplay.intValue = EditorGUILayout.Popup(targetDisplay.displayName, targetDisplay.intValue, targetDisplayOptions);

            // TODO: Use EditorGUILayout.MinMaxSlider() for horizontal and vertical to clamp values? make readonly?
            SerializedProperty cameraRect = serializedObject.FindProperty("cameraRect");
            EditorGUILayout.PropertyField(cameraRect);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
