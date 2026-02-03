using System.Collections;
using My_Assets.PuzzleScene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace My_Assets
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance;
        private int _currentLevel;
        private const string BarSceneName = "BarScene";
        private const string PuzzleSceneName = "PuzzleScene";
        private const string EndSceneName = "MainMenuScene";
        [SerializeField] public int maxLevels = 3;

        // TODO: Check if necessary
        void Start()
        {
            _currentLevel = PlayerPrefs.GetInt("Level", 1);
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                return;
            }

            Destroy(gameObject);
        }

        public void StartNewGame()
        {
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.SetInt("Drunkness", (int)Drunkness.Light);
            PlayerPrefs.SetInt("DrunknessScore", 0);

            PlayerPrefs.SetFloat("TotalTimerTime", 0);
            PlayerPrefs.Save();
            StartCoroutine(LoadScene(BarSceneName, CursorLockMode.Locked));
        }

        public void GoToNextLevel()
        {
            if (SceneManager.GetActiveScene().name == BarSceneName)
            {
                StartCoroutine(LoadScene(PuzzleSceneName, CursorLockMode.None));
                return;
            }
            _currentLevel++;

            int currentDrunkScore = PlayerPrefs.GetInt("Drunkness", 0);
            int totalDrunknessScore = PlayerPrefs.GetInt("DrunknessScore", 0);
            int newDrunknessScore = currentDrunkScore + totalDrunknessScore;
            PlayerPrefs.SetInt("DrunknessScore", newDrunknessScore);

            PlayerPrefs.SetInt("Level", _currentLevel);
            PlayerPrefs.SetInt("Drunkness", (int)Drunkness.Light);
            PlayerPrefs.Save();

            if (_currentLevel > maxLevels)
            {
                StartCoroutine(LoadScene(EndSceneName, CursorLockMode.None));
                return;
            }


            StartCoroutine(LoadScene(BarSceneName, CursorLockMode.Locked));
        }

        private IEnumerator LoadScene(string sceneName, CursorLockMode cursorLockMode)
        {
            Cursor.lockState = cursorLockMode;

            Scene currentScene = SceneManager.GetActiveScene();
            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName, mode: LoadSceneMode.Single);

            while (loadSceneOperation != null && !loadSceneOperation.isDone) yield return null;

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            SceneManager.UnloadSceneAsync(currentScene.buildIndex);
        }

        public void ReloadScene()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}