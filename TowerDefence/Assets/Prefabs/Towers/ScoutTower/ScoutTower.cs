using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutTower : TowerBaseScript
{

    public override void CheckIfEnemyIsInRange()
    {
        base.CheckIfEnemyIsInRange();
    }

    public override void FindTheNearstTarget()
    {
        base.FindTheNearstTarget();
    }

    public override void LookAtTheNearstEnemy()
    {
        base.LookAtTheNearstEnemy();
    }

    public override void ShootAtEnemy()
    {
        if (closetTarget != null && closetTarget.gameObject.layer == 8)
        {
            Debug.Log("Discovering enemy");

            closetTarget.GetComponent<ScoutEenmy>().EnemyDiscovered();
        }
        else
        {
            return;
        }
    }

}
