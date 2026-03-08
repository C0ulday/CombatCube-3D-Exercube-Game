using UnityEngine;
using UnityEngine.SceneManagement;

    public class MenuButton : MonoBehaviour
{
    public void GoToScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
