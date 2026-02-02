using TMPro;
using UnityEngine;

namespace My_Assets.PuzzleScene
{
    public class EndscreenController : MonoBehaviour
    {
        public GameSceneManager gameSceneManager;
        public BombManager bombManager;
        public GameObject succesScreen;
        public GameObject failScreen;
        private bool _bombDefused;
        private int _currentLevel;
        public TextMeshProUGUI displayText;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _bombDefused = bombManager.IsBombSolved;
            if (_bombDefused)
            {
                _currentLevel = PlayerPrefs.GetInt("Level");
            }

            GameObject toBeActivated = _bombDefused ? succesScreen : failScreen;
            toBeActivated.SetActive(true);
        }

        public void NextLevel()
        {
            gameSceneManager.GoToNextLevel();
        }

        public void RetryLevel()
        {
            gameSceneManager.ReloadScene();
        }

        public void SetDisplayText(string text)
        {
            displayText.text = text;
        }

        public void Quit()
        {
            if (_bombDefused)
            {
                int currentDrunkScore = PlayerPrefs.GetInt("Drunkness", 0);
                int totalDrunknessScore = PlayerPrefs.GetInt("DrunknessScore", 0);
                int newDrunknessScore = currentDrunkScore + totalDrunknessScore;
                PlayerPrefs.SetInt("DrunknessScore", newDrunknessScore);

                PlayerPrefs.SetInt("Level", _currentLevel);
                PlayerPrefs.SetInt("Drunkness", (int)Drunkness.Light);
                PlayerPrefs.Save();
            }
            Application.Quit();
        }
    }
}
