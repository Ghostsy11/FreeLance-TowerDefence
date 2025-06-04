using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingPoolText : MonoBehaviour
{
    public static FloatingPoolText Instance;
    public GameObject floatingTextPrefab;
    public int poolSize = 10;

    private Queue<GameObject> textPool = new Queue<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (floatingTextPrefab == null)
        {
            Debug.LogError("Floating text prefab is not assigned!");
            return;
        }

        // Pre-instantiate floating text objects and store them in the pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject textObj = Instantiate(floatingTextPrefab, transform);
            textObj.SetActive(false);
            textPool.Enqueue(textObj);
        }
    }

    public GameObject GetFloatingText()
    {
        if (textPool.Count > 0)
        {
            GameObject textObj = textPool.Dequeue();
            textObj.SetActive(true);
            return textObj;
        }
        else
        {
            // If all objects are in use, create a new one (optional)
            GameObject newText = Instantiate(floatingTextPrefab, transform);
            return newText;
        }
    }

    public void ReturnFloatingText(GameObject textObj)
    {
        textObj.SetActive(false);
        textPool.Enqueue(textObj);
    }
}
