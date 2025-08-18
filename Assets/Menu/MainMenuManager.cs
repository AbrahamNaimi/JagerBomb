using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameSceneManager gameSceneManager;
    
    public void LoadScene(string sceneName)
    {
        gameSceneManager.StartNewGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
