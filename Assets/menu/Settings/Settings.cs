using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    private bool muted = false; 

    public void GoToScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void ToggleMusic()
    {
        if (muted == false) {
            AudioListener.pause = true;
            muted = true;
        }
        else
        {
            AudioListener.pause = false;
            muted = false;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Application has quit");
    }
}
