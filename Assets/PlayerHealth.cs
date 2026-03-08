using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth;
    // One Image per camera/display — assign all 4 overlay images in the Inspector
    public Image[] healthImpacts;

    // Assign blood1 (lightest) to blood10 (heaviest) in the Inspector, index 0-9
    public Sprite[] bloodOverlays = new Sprite[10];

    // Blood overlay appears when health drops below this value
    public float lowHealthThreshold = 100f;

    void Start()
    {
        playerHealth = 200f;

        // Auto-find BloodOverlay Image if not assigned in Inspector
        if (healthImpacts == null || healthImpacts.Length == 0)
        {
            GameObject overlayGO = GameObject.Find("BloodOverlay");
            if (overlayGO != null)
            {
                Image img = overlayGO.GetComponent<Image>();
                if (img != null)
                    healthImpacts = new Image[] { img };
                else
                    Debug.LogWarning("BloodOverlay found but has no Image component.");
            }
            else
            {
                Debug.LogWarning("BloodOverlay GameObject not found. Run ExerCube > Setup Blood Overlays.");
            }
        }

        foreach (Image img in healthImpacts)
            img.enabled = false;


    }

    public void TakeDamage(int amount)
    {
        playerHealth -= amount;
        if (playerHealth < 0f) playerHealth = 0f;

        Debug.Log("Player Health: " + playerHealth);
        UpdateBloodOverlay();

        if (playerHealth <= 0f)
        {
            Die();
           
        }
    }

    void UpdateBloodOverlay()
    {
        if (playerHealth > lowHealthThreshold)
        {
            foreach (Image img in healthImpacts)
                img.enabled = false;
            return;
        }

        // Map health [0, threshold) to overlay index 0-9: lower health = higher index = more blood
        float damagePct = 1f - (playerHealth / lowHealthThreshold);
        int index = Mathf.Clamp(Mathf.FloorToInt(damagePct * 10f), 0, 9);

        foreach (Image img in healthImpacts)
        {
            if (img == null) { Debug.LogWarning("healthImpacts has a missing Image reference!"); continue; }
            img.enabled = true;
            img.color = Color.white;
            img.sprite = bloodOverlays[index];
        }
    }

    void Die()
    {
        ResultUI.Instance.ShowLose();
        Debug.Log("Player Dead");
    }
}
