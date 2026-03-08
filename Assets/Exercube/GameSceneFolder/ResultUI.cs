using UnityEngine;
using TMPro;

public class ResultUI : MonoBehaviour
{
    public static ResultUI Instance;

    [SerializeField] private GameObject canvas;
    [SerializeField] private TextMeshProUGUI resultText;

    void Awake()
    {
        Instance = this;
        canvas.SetActive(false);
    }

    public void ShowWin()
    {
        resultText.text = "YOU WON";
        canvas.SetActive(true);
        Time.timeScale = 0f; // Pause game
    }

    public void ShowLose()
    {
        resultText.text = "GAME OVER";
        resultText.color = Color.red;
        canvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
