using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ComboManager : MonoBehaviour
{
    private List<string> currentCombo = new List<string>();
    public float comboResetTime = 1.5f;
    private float lastInputTime;
    public BossMovement boss;

    // Attributs for the damage score text
    [Header("UI Damage Display")]
    public TMP_Text damageText;
    private float currentComboDamage = 0;
    private float displayTimer = 0f;
    public float displayDuration = 1.5f;

    public void OnLeftPunch(Vector3 dir) { AddInput("LeftP"); ProcessDetailedPunch(dir, 20); Debug.Log("LeftPunch"); }
    public void onRightPunch(Vector3 dir) { AddInput("RightP"); ProcessDetailedPunch(dir, 30); Debug.Log("RightPunch"); }
    public void OnLeftKick(Vector3 dir) { AddInput("LeftK"); ProcessDetailedPunch(dir, 50); Debug.Log("LeftKick"); }
    public void OnRightKick(Vector3 dir) { AddInput("RightK"); ProcessDetailedPunch(dir, 70); Debug.Log("RightKick"); }
    //Laptop
    public void OnLeftPunch() { AddInput("LeftP"); boss?.takeDamage(20, false); UpdateDamageUI(20); Debug.Log("LeftPunch"); }
    public void onRightPunch() { AddInput("RightP"); boss?.takeDamage(30, false); UpdateDamageUI(30); Debug.Log("RightPunch"); }
    public void OnLeftKick() { AddInput("LeftK"); boss?.takeDamage(50, false); UpdateDamageUI(50); Debug.Log("LeftKick"); }
    public void OnRightKick() { AddInput("RightK"); boss?.takeDamage(70, false); UpdateDamageUI(70); Debug.Log("RightKick"); }


    // Adds the name of the last input to the combo string
    private void AddInput(string input)
    {
        lastInputTime = Time.time;
        currentCombo.Add(input);
        CheckCombos();
    }

   

    private void Update()
    {
        //checks if boss object is in the scene and links it with the boss attribute. It also ignores the prefab in the scene.
        if (boss == null || !boss.gameObject.scene.IsValid())
        {
            boss = GameObject.FindObjectOfType<BossMovement>();
        }
        // Reset if too much time has passed for a combo
        if (currentCombo.Count > 0 && Time.time - lastInputTime > comboResetTime)
        {
            currentCombo.Clear();
            Debug.Log("Combo Reset");
            
        }
        // UI Timer Logik
        if (displayTimer > 0)
        {
            displayTimer -= Time.deltaTime;
            if (displayTimer <= 0)
            {
                ResetDamageUI();
            }
        }
        if (damageText != null)
        {
            damageText.transform.localScale = Vector3.Lerp(
                damageText.transform.localScale,
                Vector3.one * 1.2f,
                Time.deltaTime * 10f // shrinking rate
            );
        }
    }

    private void UpdateDamageUI(int damage)
    {
        if (damageText == null) return;

        currentComboDamage += damage;
        damageText.text = ""+currentComboDamage;
        displayTimer = displayDuration;

        // Text becomes larger when combos are hit
        damageText.transform.localScale = Vector3.one * 2f;
    }

    private void ResetDamageUI()
    {
        currentComboDamage = 0;
        if (damageText != null) damageText.text = "";
    }
     
    private void CheckCombos()
    {
        string comboString = string.Join("-", currentCombo);

        if (comboString == "LeftP-LeftP")
        {
            
            ExecuteSuperMove("Double Jab");
            boss.takeDamage(30, true);
            UpdateDamageUI(30);
            currentCombo.Clear();
        }
        else if (comboString == "LeftP-RightP-LeftP")
        {
            ExecuteSuperMove("Alternating");
            boss.takeDamage(30, true);
            UpdateDamageUI(30);
            currentCombo.Clear();
        }
        else if (comboString == "LeftP-RightP-RightK")
        {
            ExecuteSuperMove("Finishing Kick");
            boss.takeDamage(30, true);
            UpdateDamageUI(30);
            currentCombo.Clear();
        }

    }

    void ExecuteSuperMove(string moveName)
    {
        Debug.Log("SUPER MOVE: " + moveName);
    }

    // Calculates whether a punch or kick was in the right direction to hit an enemy or the boss
    // Using dot product the angle between object and player punch direction is calculated
    // The angle cannot be lower than 0.85f
    public void ProcessDetailedPunch2(Vector3 punchDir, int amountOfDamage)
    {
        IDamageable bestTarget = null;
        float highestPrecision = -1f;

        // Searches for small enemies or boss
        Vector3 correctedPunchDir = Quaternion.Euler(0, -90, 0) * punchDir;
        MonoBehaviour[] allSceneObjects = Object.FindObjectsOfType<MonoBehaviour>();

        foreach (MonoBehaviour mb in allSceneObjects)
        {
            if (mb is IDamageable target)
            {
                Vector3 dirToTarget = Vector3.ProjectOnPlane(mb.transform.position - transform.position, Vector3.up).normalized;
                float precision = Vector3.Dot(punchDir.normalized, dirToTarget);
                Debug.Log("damageable found: " + mb.name);

                if (precision > 0.85f && precision > highestPrecision)
                {
                    highestPrecision = precision;
                    bestTarget = target;
                }
            }
        }

        bestTarget?.takeDamage(amountOfDamage, false);
        UpdateDamageUI(amountOfDamage);

    }

    public void ProcessDetailedPunch(Vector3 punchDir, int amountOfDamage)
    {
        IDamageable bestTarget = null;
        float highestPrecision = -1f;

        // Nutze lieber Interfaces direkt, falls deine Unity Version das unterstützt, 
        // ansonsten bleiben wir bei MonoBehaviour aber filtern besser.
        MonoBehaviour[] allSceneObjects = Object.FindObjectsOfType<MonoBehaviour>();

        foreach (MonoBehaviour mb in allSceneObjects)
        {
            if (mb is IDamageable target)
            {
                // Vektor vom Spieler zum Gegner
                Vector3 offset = mb.transform.position - transform.position;
                // Wir ignorieren den Höhenunterschied für die Cosine Similarity
                Vector3 dirToTarget = Vector3.ProjectOnPlane(offset, Vector3.up).normalized;

                // WICHTIG: punchDir muss auch auf der Plane sein
                Vector3 flatPunchDir = Vector3.ProjectOnPlane(punchDir, Vector3.up).normalized;

                float precision = Vector3.Dot(flatPunchDir, dirToTarget);

                // Debug Hilfe: Zeichne eine Linie im Editor
                Debug.DrawRay(transform.position, dirToTarget * 5, Color.red, 2f);
                Debug.DrawRay(transform.position, flatPunchDir * 5, Color.blue, 2f);

                // 0.85f ist ca 31 Grad. Für den Boss evtl auf 0.7f (45 Grad) gehen?
                if (precision > 0.7f && precision > highestPrecision)
                {
                    highestPrecision = precision;
                    bestTarget = target;
                }
            }
        }

        if (bestTarget != null)
        {
            bestTarget.takeDamage(amountOfDamage, false);
            Debug.Log("Hit: " + bestTarget.ToString() + " with precision: " + highestPrecision);
        }
    }


}
