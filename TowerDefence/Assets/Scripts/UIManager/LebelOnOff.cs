using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LebelOnOff : MonoBehaviour
{
    [SerializeField] List<CoordinateLabeler> coordinateLabelers = new List<CoordinateLabeler>();
    [SerializeField] GameObject[] lables;

    [SerializeField] bool isOn;

    private void Awake()
    {

    }

    private void Start()
    {

        lables = GameObject.FindGameObjectsWithTag("Lebel");

        foreach (var lable in lables)
        {
            lable.gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            LebelIsOnOff();
        }
    }

    private void LebelIsOnOff()
    {
        isOn = !isOn;
        if (!isOn)
        {
            foreach (var lable in lables)
            {
                lable.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var lable in lables)
            {
                lable.gameObject.SetActive(false);
            }
        }

    }

}
