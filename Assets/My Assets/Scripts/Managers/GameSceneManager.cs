using System.Collections;
using Puzzles.Puzzle_Generation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace My_Assets.Managers
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance;
        private int _currentLevel;
        [SerializeField] private string barSceneName = "BarSceneNew";
        [SerializeField] private string puzzleSceneName = "Puzzle 01";
        [SerializeField] private string endSceneName = "EndScene";
        [SerializeField] private int maxLevels = 3;

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
            PlayerPrefs.Save();
            StartCoroutine(LoadScene(barSceneName, CursorLockMode.Locked));
        }

        public void GoToNextLevel()
        {
            if (SceneManager.GetActiveScene().name == barSceneName)
            {
                StartCoroutine(LoadScene(puzzleSceneName, CursorLockMode.None));
                return;
            }
            _currentLevel++;
            PlayerPrefs.SetInt("Level", _currentLevel);
            PlayerPrefs.SetInt("Drunkness", (int)Drunkness.Light);
            PlayerPrefs.Save();

            if (_currentLevel > maxLevels)
            {
                StartCoroutine(LoadScene(endSceneName, CursorLockMode.None));
                return;
            }


            StartCoroutine(LoadScene(barSceneName, CursorLockMode.Locked));
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