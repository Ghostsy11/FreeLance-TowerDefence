using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float fadeDuration = 1f;
    public float maxScale = 1.5f;

    private TextMeshPro textMesh;
    private Color textColor;
    private Vector3 originalScale;
    private float timer = 0f;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        if (textMesh == null)
        {
            Debug.LogError("TextMeshPro component missing on FloatingText prefab!");
        }
        originalScale = transform.localScale;
    }

    void OnEnable()
    {
        timer = 0f;
        transform.localScale = originalScale;
        textColor.a = 1f;
        textMesh.color = textColor;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float progress = timer / fadeDuration;

        // Move up
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // Scale effect (grow then shrink)
        float scale = Mathf.Lerp(maxScale, originalScale.x, progress);
        transform.localScale = new Vector3(scale, scale, scale);

        // Fade out effect
        textColor.a = Mathf.Lerp(1f, 0f, progress);
        textMesh.color = textColor;

        if (progress >= 1f)
        {
            FloatingPoolText.Instance.ReturnFloatingText(gameObject);
        }
    }

    public void SetText(string value)
    {
        textMesh.text = value;
    }

    public void SetColor(Color color)
    {
        textColor = color;
        textMesh.color = textColor;
    }

}