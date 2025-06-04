using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenHealthDisplayer : MonoBehaviour
{
    public Scrollbar castle;
    private void Start()
    {
        castle = GameObject.Find("HealthBar").GetComponent<Scrollbar>();

        UpdateScreenHealth();
    }
    public void UpdateScreenHealth()
    {
        gameObject.GetComponent<Scrollbar>().size = castle.size;

    }

}

