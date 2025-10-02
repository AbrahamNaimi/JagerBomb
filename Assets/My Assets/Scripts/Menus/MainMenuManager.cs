using My_Assets.Managers;
using UnityEngine;

namespace My_Assets.Menus
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
