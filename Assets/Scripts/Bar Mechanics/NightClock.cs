using UnityEngine;
using TMPro;

public class NightClock : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text label;     // Drag your TMP Text here (or it’ll try GetComponent)

    [Header("Time Range (12h AM/PM)")]
    [SerializeField] private int startHour = 9; // 9 PM
    [SerializeField] private int startMinute = 0;
    [SerializeField] private bool startIsPM = true;

    [SerializeField] private int endHour = 7;   // 7 AM
    [SerializeField] private int endMinute = 0;
    [SerializeField] private bool endIsPM = false;

    [Header("Playback")]
    [Tooltip("How many real-time seconds should it take to go from start to end.")]
    [Min(0.01f)]
    [SerializeField] private float durationSeconds = 3f;

    [Tooltip("If true, it loops back to start when it reaches the end; otherwise it stops at the end.")]
    [SerializeField] private bool loop = true;

    private float elapsed;
    private int startTotalMin;
    private int spanMin;

    private bool finishedTriggered;   // <-- so we only call once

    void Awake()
    {
        if (label == null) label = GetComponent<TMP_Text>();

        startTotalMin = ConvertTo24HourMinutes(startHour, startMinute, startIsPM);
        int endTotalMin = ConvertTo24HourMinutes(endHour, endMinute, endIsPM);

        const int day = 24 * 60;
        spanMin = ((endTotalMin - startTotalMin) % day + day) % day;
        if (spanMin == 0) spanMin = day; // Full day if same time

        UpdateLabel(startTotalMin);
    }

    void Update()
    {
        if (durationSeconds <= 0f) return;

        if (loop)
        {
            elapsed = (elapsed + Time.deltaTime) % durationSeconds;
        }
        else
        {
            if (elapsed < durationSeconds)
            {
                elapsed = Mathf.Min(elapsed + Time.deltaTime, durationSeconds);

                // Call method when finished
                if (elapsed >= durationSeconds && !finishedTriggered)
                {
                    finishedTriggered = true;
                    OnTimerFinished();
                }
            }
        }

        float t = Mathf.Clamp01(elapsed / durationSeconds);
        float currentMinFloat = startTotalMin + t * spanMin;
        int totalMin = ((int)Mathf.Floor(currentMinFloat)) % (24 * 60);

        UpdateLabel(totalMin);
    }

    private void UpdateLabel(int totalMin)
    {
        int hour24 = (totalMin / 60) % 24;
        int minute = totalMin % 60;

        // Convert to 12-hour format with AM/PM
        string ampm = hour24 >= 12 ? "PM" : "AM";
        int hour12 = hour24 % 12;
        if (hour12 == 0) hour12 = 12;

        if (label != null)
            label.text = $"{hour12:D2}:{minute:D2} {ampm}";
    }

    private int ConvertTo24HourMinutes(int hour12, int minute, bool isPM)
    {
        int hour24 = hour12 % 12; // 12 PM or AM is handled below
        if (isPM) hour24 += 12;
        return hour24 * 60 + minute;
    }

    // 🔔 This gets called when timer ends (only if loop == false)
    private void OnTimerFinished()
    {
        Debug.Log("NightClock finished!");
    }
}
