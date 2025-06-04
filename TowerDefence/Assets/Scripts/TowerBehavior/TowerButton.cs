using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButton : MonoBehaviour
{

    public GameObject towerToPlace;
    public GameObject towerIndecator;

    public void SelectedTower()
    {
        Debug.Log("Pressed");

        TowerManager.instance.StartTowerPlacemeant(towerToPlace, towerIndecator);
    }

}
