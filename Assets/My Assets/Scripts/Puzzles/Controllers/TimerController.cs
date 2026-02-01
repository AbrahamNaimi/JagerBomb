using Puzzles;
using UnityEngine;
using UnityEngine.UI;

namespace My_Assets.Puzzles.Controllers
{
    public class TimerController : MonoBehaviour
    {
        public TextMesh timerText;
        public bool isStopped = true;
        public BombManager _BombManager;

        private float _currentTimerTime;
        private float initialTime;

        // Update is called once per frame
        void Update()
        {

            if (isStopped) return;
            _currentTimerTime -= Time.deltaTime;
            if (_currentTimerTime < 0) _currentTimerTime = 0;

            timerText.text = formatTime(_currentTimerTime);

            if (_currentTimerTime == 0)
            {
                _BombManager.ExplodeBomb();
            }
        }

        public void StartTimer(float startTimeInSeconds)
        {
            initialTime = startTimeInSeconds;
            _currentTimerTime = startTimeInSeconds;
            isStopped = false;
        }

        public void DeductTime(float time)
        {
            _currentTimerTime -= time;
        }

        public void PauseTimer()
        {
            isStopped = true;
        }

        public string getCurrentTimerTimeFormatted()
        {
            return formatTime(initialTime - _currentTimerTime);
        }

        public float getCurrentTimerTime()
        {
            return initialTime - _currentTimerTime;
        }

        public string formatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            int centiseconds = Mathf.FloorToInt((time * 100) % 100);

            string text = $"{minutes:D2}:{seconds:D2}:{centiseconds:D2}";
            return text;
        }
    }
}
