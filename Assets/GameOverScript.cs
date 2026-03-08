using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    public static GameOverScript Instance;
    public string GameOverText = "GAME OVER";
    [SerializeField] private GameObject gameOverCanvas;

    void Awake()
    {
        Instance = this;
        gameOverCanvas.SetActive(false);
    }

    public void ShowGameOver(bool win)
    {
        if (win)
        {
            GameOverText = "YOU WON";
        }
        gameOverCanvas.SetActive(true);
        

    }
}
