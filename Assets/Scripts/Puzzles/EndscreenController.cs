using Puzzles.Puzzle_Generation;
using UnityEngine;

namespace Puzzles
{
    public class EndscreenController : MonoBehaviour
    { 
        public GameSceneManager gameSceneManager;
        public BombManager bombManager;
        public GameObject succesScreen;
        public GameObject failScreen;
 
        private bool _bombDefused;
        private int _currentLevel;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _bombDefused = bombManager.IsBombSolved;
            if (_bombDefused)
            {
                _currentLevel = PlayerPrefs.GetInt("Level");
            }
            
            GameObject toBeActivated = _bombDefused ?  succesScreen : failScreen;
            toBeActivated.SetActive(true);
        }

        public void NextLevel()
        {
            PlayerPrefs.SetInt("Level", _currentLevel + 1);
            PlayerPrefs.SetInt("Drunkness", (int)Drunkness.Light);
            PlayerPrefs.Save();

            gameSceneManager.GoToNextLevel();
        }

        public void RetryLevel()
        {
            gameSceneManager.LoadCurrentLevelScene();
        }

        public void Quit()
        {
            if (_bombDefused)
            {
                PlayerPrefs.SetInt("Level", _currentLevel);
                PlayerPrefs.SetInt("Drunkness", (int)Drunkness.Light);
                PlayerPrefs.Save();
            }
            Application.Quit();
        }
    }
}
