using UnityEngine;

namespace Puzzles
{
    public class TimerController : MonoBehaviour
    {
        public TextMesh timerText;
        public bool isStopped = true;

        private float _currentTimerTime;

        // Update is called once per frame
        void Update()
        {
            if (isStopped) return;
            _currentTimerTime -= Time.deltaTime;
            if (_currentTimerTime < 0) _currentTimerTime = 0;

            int minutes = Mathf.FloorToInt(_currentTimerTime / 60);
            int seconds = Mathf.FloorToInt(_currentTimerTime % 60);
            int centiseconds = Mathf.FloorToInt((_currentTimerTime * 100) % 100);

            timerText.text = $"{minutes:D2}:{seconds:D2}:{centiseconds:D2}";

            if (_currentTimerTime == 0)
            {
                
            }
        }

        public void StartTimer(float startTimeInSeconds)
        {
            _currentTimerTime = startTimeInSeconds;
            isStopped = false;
        }

        public void DeductTime(float time)
        {
            _currentTimerTime -= time;
        }

        public void PauseUnpauseTimer()
        {
            isStopped = !isStopped;
        }
    }
}
