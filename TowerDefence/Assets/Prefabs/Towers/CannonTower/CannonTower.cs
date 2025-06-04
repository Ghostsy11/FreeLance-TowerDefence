using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : TowerBaseScript
{
    [SerializeField] TowerDataSO cannonTowerSo;
    private ParticleSystem impactEffect;
    private AudioSource impactSoundEffect;


    private void Awake()
    {
        impactSoundEffect = gameObject.GetComponent<AudioSource>();

    }
    private void Start()
    {

        impactEffect = Instantiate(cannonTowerSo.attackEffect, firePoint.transform.position, firePoint.transform.rotation, firePoint);
        impactSoundEffect.clip = cannonTowerSo.attackSound;


        towerHitRange = cannonTowerSo.attackRange;
        fireRate = cannonTowerSo.attackSpeed;

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
