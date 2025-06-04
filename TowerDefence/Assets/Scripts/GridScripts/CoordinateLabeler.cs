using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]


public class CoordinateLabeler : MonoBehaviour
{

    TextMeshPro nodeCoordinateText;
    Vector2Int gameObjectCoordinate = new Vector2Int();

    private void Awake()
    {

        nodeCoordinateText = GetComponent<TextMeshPro>();
        DisplayCoordinates();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateCoordinateName();

        }
        else
        {
            return;
        }
    }

    private void DisplayCoordinates()
    {

        // / UnityEditor.EditorSnapSettings.move.x
        // / UnityEditor.EditorSnapSettings.move.z
        gameObjectCoordinate.x = Mathf.RoundToInt(transform.parent.position.x);
        gameObjectCoordinate.y = Mathf.RoundToInt(transform.parent.position.z);

        nodeCoordinateText.text = "(" + gameObjectCoordinate.x + "," + gameObjectCoordinate.y + ")";
    }

    private void UpdateCoordinateName()
    {
        transform.parent.name = gameObjectCoordinate.ToString();
    }

}
