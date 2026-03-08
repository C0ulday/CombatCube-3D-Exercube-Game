using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class PlayerHealthSetup
{
    [MenuItem("ExerCube/Setup Blood Overlays")]
    static void SetupBloodOverlays()
    {
        PlayerHealth playerHealth = Object.FindObjectOfType<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("No PlayerHealth component found in scene.");
            return;
        }

        // --- Assign sprites ---
        Sprite[] sprites = new Sprite[10];
        bool allFound = true;

        for (int i = 0; i < 10; i++)
        {
            string path = $"Assets/Exercube/GameScene/BloodOverlay/blood{i + 1}.png";
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

            if (sprite == null)
            {
                Debug.LogError($"Could not find sprite at: {path} — make sure Texture Type is set to Sprite (2D and UI)");
                allFound = false;
            }

            sprites[i] = sprite;
        }

        if (!allFound) return;

        // --- Find or create a dedicated BloodOverlay Canvas at the root level ---
        GameObject existingCanvasGO = GameObject.Find("BloodOverlayCanvas");
        Canvas canvas;
        if (existingCanvasGO != null)
        {
            canvas = existingCanvasGO.GetComponent<Canvas>();
        }
        else
        {
            GameObject canvasGO = new GameObject("BloodOverlayCanvas");
            Undo.RegisterCreatedObjectUndo(canvasGO, "Create BloodOverlayCanvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }

        // --- Find or create BloodOverlay Image ---
        Transform existing = canvas.transform.Find("BloodOverlay");
        GameObject overlayGO;
        if (existing != null)
        {
            overlayGO = existing.gameObject;
        }
        else
        {
            overlayGO = new GameObject("BloodOverlay");
            Undo.RegisterCreatedObjectUndo(overlayGO, "Create BloodOverlay");
            overlayGO.transform.SetParent(canvas.transform, false);
        }

        // Stretch to fill the entire screen
        Image overlayImage = overlayGO.GetComponent<Image>() ?? overlayGO.AddComponent<Image>();

        RectTransform rt = overlayGO.GetComponent<RectTransform>();
        Undo.RecordObject(rt, "Setup Blood Overlays");
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        Undo.RecordObject(overlayImage, "Setup Blood Overlays");
        overlayImage.color = Color.white;
        overlayImage.raycastTarget = false;
        overlayImage.enabled = false;

        // --- Wire everything up on PlayerHealth ---
        Undo.RecordObject(playerHealth, "Setup Blood Overlays");
        playerHealth.bloodOverlays = sprites;
        playerHealth.healthImpacts = new Image[] { overlayImage };
        EditorUtility.SetDirty(playerHealth);
        EditorUtility.SetDirty(overlayImage);
        EditorUtility.SetDirty(rt);

        Debug.Log("Blood Overlays and UI overlay assigned successfully. Run ExerCube > Setup Blood Overlays again if you add more displays.");
    }
}
