using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TowerBehavior", menuName = "ScriptableObjects/Tower Defense Arrow Level One", order = 1)]
public class TowerDataSO : ScriptableObject
{
    [Header("Tower Info")]
    public string towerName;

    [Header("Stats")]
    public int goldCost;
    public int stoneCost;
    public float attackRange;
    public float attackSpeed;

    [Header("Upgrade")]
    public int upgradeCost;
    public GameObject nextUpgrade;

    [Header("Effects & Audio")]
    public AudioClip attackSound;
    public ParticleSystem attackEffect;

}
