using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameProgressTimer : MonoBehaviour
{
    public TextMeshProUGUI lastRecord; // Assign in Inspector or get in Start()

    void Start()
    {
        if (lastRecord == null)
        {
            lastRecord = GetComponent<TextMeshProUGUI>(); // Try getting the component automatically
        }

        if (lastRecord != null)
        {
            float recordTime = GameTimer.Instance.GetLastRecord();
            int minutes = Mathf.FloorToInt(recordTime / 60);
            int seconds = Mathf.FloorToInt(recordTime % 60);
            lastRecord.text = string.Format("Last Record: {0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            Debug.LogError("TextMeshProUGUI reference is missing in GameProgressTimer.");
        }
    }
}
