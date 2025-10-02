using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameSceneManager gameSceneManager;
    
    public void LoadScene()
    {
        gameSceneManager.StartNewGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
