using UnityEngine;

namespace My_Assets.Main_menu
{
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
}
