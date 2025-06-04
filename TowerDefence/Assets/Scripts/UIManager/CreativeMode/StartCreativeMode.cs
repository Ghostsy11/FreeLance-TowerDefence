using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCreativeMode : MonoBehaviour
{

    private void Awake()
    {
        Time.timeScale = 0f;
    }

    public void Confirem()
    {
        gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

}
