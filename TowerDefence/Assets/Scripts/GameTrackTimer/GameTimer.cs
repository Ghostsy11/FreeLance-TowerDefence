using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;
    public TextMeshProUGUI timerText; // Assign your UI Text in the Inspector
    private float elapsedTime = 0f;
    public float lastRecordedTime = 0f; // Stores the last recorded time

    private void Start()
    {
        Instance = this;
    }

    void Update()
    {
        KeepTrackOfTime();
    }

    void KeepTrackOfTime()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public float SaveGameTime()
    {
        float previousRecord = PlayerPrefs.GetFloat("LastRecord", 0f); // Get the last saved record

        if (elapsedTime > previousRecord) // Only update if the new time is higher
        {
            lastRecordedTime = elapsedTime;
            PlayerPrefs.SetFloat("LastRecord", lastRecordedTime);
            PlayerPrefs.Save();
        }

        return PlayerPrefs.GetFloat("LastRecord", 0f); // Return the latest record
    }

    public float GetLastRecord()
    {
        return PlayerPrefs.GetFloat("LastRecord", 0f); // Return saved time, default to 0
    }

}
