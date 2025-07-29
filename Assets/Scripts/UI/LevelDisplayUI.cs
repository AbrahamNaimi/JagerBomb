using TMPro;
using UnityEngine;

// To use this:
// Add Canvas, add component LevelDisplayUI script
// Under Canvas, add text and drag this element to the component above

public class LevelDisplayUI : MonoBehaviour
{

    [SerializeField] private TMP_Text levelText;

    void Start()
    {
        int currentLevel = GameSceneManager.Instance.GetCurrentLevel();
        levelText.text = $"Workday {currentLevel}";
    }
}
