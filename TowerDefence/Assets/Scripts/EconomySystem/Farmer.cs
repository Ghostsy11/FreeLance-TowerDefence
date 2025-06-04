using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour
{

    public float minTime = 5f;
    public float maxTime = 15f;

    public int minGoldAmount = 1;
    public int maxGoldAmount = 3;

    public int minStoneAmount = 1;
    public int maxStoneAmount = 3;

    private bool isActive = true;

    [SerializeField] GameObject textInstance;
    [SerializeField] FloatingText floatingText;
    void Start()
    {
        if (gameObject.CompareTag("Gold"))
        {
            StartCoroutine(FarmResourceOverTime("Gold"));
        }
        else if (gameObject.CompareTag("Stone"))
        {
            StartCoroutine(FarmResourceOverTime("Stone"));
        }

        transform.position = new Vector3(transform.position.x, 0.05f, transform.position.z);
    }

    private IEnumerator FarmResourceOverTime(string resourceType)
    {
        while (isActive)
        {
            float waitTime = GetRandomTime();
            yield return new WaitForSeconds(waitTime);

            int earnedAmount = 0;

            if (resourceType == "Gold")
            {
                earnedAmount = Random.Range(minGoldAmount, maxGoldAmount);
                EconomyManager.Instance.Gold += earnedAmount;
            }
            else if (resourceType == "Stone")
            {
                earnedAmount = Random.Range(minStoneAmount, maxStoneAmount);
                EconomyManager.Instance.Stone += earnedAmount;
            }

            ShowFloatingText(earnedAmount, resourceType);
        }
    }

    private float GetRandomTime()
    {
        return Random.Range(minTime, maxTime);
    }

    private void ShowFloatingText(int amount, string resourceType)
    {
        if (FloatingPoolText.Instance == null)
        {
            Debug.LogError("FloatingPoolText instance is missing in the scene!");
            return;
        }

        textInstance = FloatingPoolText.Instance.GetFloatingText();
        if (textInstance == null)
        {
            Debug.LogError("FloatingPoolText returned a null object!");
            return;
        }

        // Set position dynamically above the building
        textInstance.transform.position = transform.position + new Vector3(0, 2f, 0);

        floatingText = textInstance.GetComponent<FloatingText>();
        floatingText.SetText("+" + amount);

        // Apply color based on the resource type
        if (resourceType == "Gold")
        {

            floatingText.SetColor(Color.yellow);
        }
        else if (resourceType == "Stone")
        {

            floatingText.SetColor(Color.gray);
        }
    }

}
