using UnityEngine;

public class TimerController : MonoBehaviour
{
    public float startTimeInSeconds = 120.00f;
    public TextMesh timerText;

    private float _currentTimerTime;

    void Start()
    {
        _currentTimerTime = startTimeInSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        _currentTimerTime -= Time.deltaTime;
        if (_currentTimerTime < 0) _currentTimerTime = 0;

        int minutes = Mathf.FloorToInt(_currentTimerTime / 60);
        int seconds = Mathf.FloorToInt(_currentTimerTime % 60);
        int centiseconds = Mathf.FloorToInt((_currentTimerTime * 100) % 100);

        timerText.text = $"{minutes:D2}:{seconds:D2}:{centiseconds:D2}";
    }
}
