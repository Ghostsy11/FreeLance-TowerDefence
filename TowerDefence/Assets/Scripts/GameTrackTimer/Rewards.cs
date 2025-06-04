using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Rewards : MonoBehaviour
{
    public static Rewards Instance;
    public int totalKills;
    private TextMeshProUGUI kills;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        kills = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            totalKills += 95;
        }
    }

    public void UpdateKills()
    {
        totalKills += 1;
        kills.text = totalKills.ToString();
    }
}
