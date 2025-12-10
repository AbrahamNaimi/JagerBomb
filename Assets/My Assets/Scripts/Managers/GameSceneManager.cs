using Puzzles.Puzzle_Generation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace My_Assets.Managers
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance;
        private int _currentLevel;
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

        // Call this method from main menu to begin a new game
        public void StartNewGame()
        {
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.SetInt("Drunkness", (int)Drunkness.Light);
            PlayerPrefs.Save();
            LoadBarScene();
        }

        // Call this method to load Bar scene
        private void LoadBarScene()
        {
            SceneManager.LoadSceneAsync("BarSceneNew", mode: LoadSceneMode.Single);
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        private void LoadPuzzleScene()
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadSceneAsync("Puzzle 01", mode: LoadSceneMode.Single);
        }

        // Method used for loading level scene
        public void LoadCurrentLevelScene()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadSceneAsync(sceneName);
        }

        // Call this method after completing a level
        public void GoToNextLevel()
        {
            if (SceneManager.GetActiveScene().name == "BarSceneNew")
            {
                Cursor.lockState = CursorLockMode.None;
                LoadPuzzleScene();
                return;
            }
            _currentLevel++;
            PlayerPrefs.SetInt("Level", _currentLevel);
            PlayerPrefs.SetInt("Drunkness", (int)Drunkness.Light);
            PlayerPrefs.Save();
        
            if (_currentLevel > maxLevels)
            {
                SceneManager.LoadScene("EndScene", mode: LoadSceneMode.Single);
                return;
            }

            Cursor.lockState = CursorLockMode.Locked;
            LoadBarScene();
        }

        // Call this method after failing a level
        public void Explode()
        {
            SceneManager.LoadSceneAsync("EndScene", mode: LoadSceneMode.Single);
        }
    }
}