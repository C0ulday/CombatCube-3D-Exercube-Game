using System.IO;
using UnityEngine;

// TODO: Handle editor mode/play mode loading/saving/resetting

namespace Sphery.ExerCube
{
    [CreateAssetMenu(menuName = "ExerCube/Display", fileName = "Display.asset")]
    public class Display : ScriptableObject
    {
        //public string filePath => Path.Combine("C:/Sphery/Shared/Displays/", name + ".json");
        public string filePath => Path.Combine(Application.streamingAssetsPath, "Displays", name + ".json");

        public string displayName;
        public int targetDisplay = 0;
        public Rect cameraRect = new Rect(0f, 0f, 1f, 1f);

        [ContextMenu("Load")]
        public void Load()
        {
            // Load player prefs from file
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                JsonUtility.FromJsonOverwrite(json, this);
            }
        }

        [ContextMenu("Save")]
        public void Save()
        {
            // Create directory if it doesn't exist
            string directory = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            // Write player prefs to file
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(filePath, json);
        }
    }
}
