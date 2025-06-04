using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCost : MonoBehaviour
{
    [SerializeField] TowerDataSO towerCost;



    public void TakeTowerCost()
    {

        EconomyManager.Instance.Gold -= towerCost.goldCost;
        EconomyManager.Instance.Stone -= towerCost.stoneCost;

    }

    public void ReturnBuildingCost()
    {
        EconomyManager.Instance.Gold += towerCost.goldCost;
        EconomyManager.Instance.Stone += towerCost.stoneCost;
    }

    public bool VirfiyTowerCost()
    {
        if (EconomyManager.Instance.Gold >= towerCost.goldCost && EconomyManager.Instance.Stone >= towerCost.stoneCost)
        {
            TakeTowerCost();
            return true;
        }
        return false;
    }

}
