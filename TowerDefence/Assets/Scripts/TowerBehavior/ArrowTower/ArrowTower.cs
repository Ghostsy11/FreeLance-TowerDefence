using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : TowerBaseScript
{
    [SerializeField] TowerDataSO arrowTowerSo;
    private ParticleSystem impactEffect;
    private AudioSource impactSoundEffect;


    private void Awake()
    {
        impactSoundEffect = gameObject.GetComponent<AudioSource>();
        towerHitRange = arrowTowerSo.attackRange;


    }
    private void Start()
    {

        impactEffect = Instantiate(arrowTowerSo.attackEffect, firePoint.transform.position, firePoint.transform.rotation, gameObject.transform);
        impactSoundEffect.clip = arrowTowerSo.attackSound;


        fireRate = arrowTowerSo.attackSpeed;

    }

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

        base.ShootAtEnemy();

        if (!impactEffect.isPaused)
        {
            impactEffect.Play();
        }
        impactSoundEffect.Play();


    }

    public override bool CheckTimerIsComplete()
    {
        return base.CheckTimerIsComplete();
    }

    public override bool CheckTimerBetweenEveryShot()
    {
        return base.CheckTimerBetweenEveryShot();

    }

}
