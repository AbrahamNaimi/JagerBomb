using System.Collections;
using Puzzles.Puzzle_Generation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;
    private int currentLevel = 1;
    [SerializeField] private int maxLevels = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);

    }

    // Simulate "New Game" button press in main menu
    // WARNING: only for testing — remove later
    private IEnumerator Start()
    {
        if (SceneManager.GetActiveScene().name == "TestStartScene")
        {
            yield return new WaitForSeconds(5f);
            StartNewGame();
        }
    }

    // Call this method from main menu to begin a new game
    public void StartNewGame()
    {
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.SetInt("Drunkness", (int) Drunkness.Light);
        PlayerPrefs.Save();
        LoadBarScene();
    }

    // Call this method to load Bar scene
    private void LoadBarScene()
    {
        SceneManager.LoadScene("BarScene");
    }

    // Method used for loading level scene
    public void LoadCurrentLevelScene()
    {
        string sceneName = GetLevelSceneName(currentLevel);
        Debug.Log("Loading current level scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    // Call this method after completing a level
    public void GoToNextLevel()
    {
        currentLevel++;
        Debug.Log("GoToNextLevel called, currentLevel: " + currentLevel);

        if (currentLevel > maxLevels)
        {
            SceneManager.LoadScene("EndScene");
            return;
        }

        LoadBarScene();
    }

    // Call this method after failing a level
    public void Explode()
    {
        SceneManager.LoadScene("EndScene");
    }

    // Method for converting levelNumber to corresponding scene name
    private string GetLevelSceneName(int levelNumber)
    {
        return $"LevelDay{levelNumber}";
    }

    // Method for retrieving current level
    public int GetCurrentLevel() => currentLevel;
    
}
