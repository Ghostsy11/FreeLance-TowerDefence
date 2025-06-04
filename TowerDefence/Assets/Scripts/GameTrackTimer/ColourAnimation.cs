using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourAnimation : MonoBehaviour
{
    public Image image; // Assign your UI Image in the Inspector
    public Color[] colors = new Color[4]; // Array to store four colors
    private int currentColorIndex = 0;
    private float duration = 15f; // 60 seconds divided by 4 colors = 15 seconds per transition

    void Start()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        if (colors.Length < 4)
        {
            Debug.LogError("Please assign four colors in the Inspector.");
            return;
        }

        StartCoroutine(ColorLerpCoroutine());
    }

    IEnumerator ColorLerpCoroutine()
    {
        while (true)
        {
            Color startColor = colors[currentColorIndex];
            Color endColor = colors[(currentColorIndex + 1) % colors.Length];
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                image.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            image.color = endColor;
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
        }
    }
}
