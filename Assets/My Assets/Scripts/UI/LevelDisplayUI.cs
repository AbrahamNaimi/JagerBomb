using TMPro;
using UnityEngine;

// To use this:
// Add Canvas, add component LevelDisplayUI script
// Under Canvas, add text and drag this element to the component above

namespace My_Assets.UI
{
    public class LevelDisplayUI : MonoBehaviour
    {

        [SerializeField] private TMP_Text levelText;

        void Start()
        {
            int currentLevel = PlayerPrefs.GetInt("Level", 1);
            levelText.text = $"Workday {currentLevel}";
        }
    }
}
