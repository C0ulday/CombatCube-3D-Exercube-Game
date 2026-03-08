using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
    public static LevelProgress Instance;

    public LevelLogic logic;

    public Image progressBar;
    public Text levelText;

    public int currentKills = 0;
    public int requiredKills = 2;
    public int level = 1;
    public int bossLevel = 4;
    public float levelSpeed = 8f; //4
    public int levelHealth = 100;
    private bool gameEnded = false;
    public Color barColor = Color.green;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
        logic = GetComponent<LevelLogic>();
    }

    public void AddKill()
    {
        if (gameEnded || level>=bossLevel)
        {
            return;
        }
        currentKills++;

        if (currentKills >= requiredKills)
        {
            LevelUp();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (level == bossLevel) {
            levelText.text = "BOSS LEVEL";
            barColor = Color.red;
            progressBar.fillAmount = 1f;
        }
        else {
            levelText.text = "LEVEL " + level;
            progressBar.fillAmount = (float)currentKills / requiredKills;
        }
        progressBar.color = barColor;

        if (gameEnded)
        {
            levelText.text = "YOU WON, CONGRATULATIONS!";
            return;
        }
        

    }

    public void updateBoss(float ratio)
    {
        progressBar.fillAmount = ratio;
        return;

    }

    void LevelUp()
    {
        level++;
        currentKills = 0;
        requiredKills += 3;
        levelSpeed *= 1.25f;
        levelHealth += 50;

        Debug.Log("LEVEL UP! NOW LEVEL " + level);
    }

    public float LevelSpeed()
    {
        return levelSpeed;
    }

    public int LevelHealth()
    {
        return levelHealth;
    }

    void WinGame()
    {
        gameEnded = true;
        progressBar.fillAmount = 1f;
        Debug.Log("GAME WON");
    }

    public bool endGame()
    {
        return gameEnded;
    }

}
