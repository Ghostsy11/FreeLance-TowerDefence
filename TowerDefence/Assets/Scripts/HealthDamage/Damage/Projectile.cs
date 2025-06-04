using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody bulletRigidbody;
    [SerializeField] float DamageEnemy = 50f;
    [SerializeField] float bulletSpeed;
    EnemyHealth enemyCurrentHealth;
    float timeToDestroy;

    [SerializeField] GameObject bulletPrefab;

    [SerializeField] AudioSource deathEffect;


    public void Initialize(GameObject prefab)
    {
        bulletPrefab = prefab;
    }

    private void Start()
    {
        deathEffect = GetComponent<AudioSource>();
        bulletRigidbody = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        timeToDestroy += Time.deltaTime;
        FireProjectile();
        DestroyProjectile();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        if (other != null)
        {
            if (other.gameObject.TryGetComponent<EnemyHealth>(out enemyCurrentHealth))
            {
                enemyCurrentHealth.DamageEnemy = DamageEnemy;
                enemyCurrentHealth.OnHit();
                if (enemyCurrentHealth.objectCurrentHealth() > 0)
                {
                    if (!enemyCurrentHealth.onHurtEffect.isPlaying)
                    {
                        enemyCurrentHealth.onHurtEffect.transform.position = transform.position;
                        enemyCurrentHealth.onHurtEffect.Play();
                    }
                    ReturnBulletToObject();


                }
                else
                {
                    if (!deathEffect.isPlaying)
                    {
                        deathEffect.Play();
                    }
                    EconomyManager.Instance.KillReward(5, 5);
                    Rewards.Instance.UpdateKills();
                    EnemiesPool.Instance.DeactivateEnemy(enemyCurrentHealth.gameObject);
                    enemyCurrentHealth.ResetHealth();
                    GameManager.instance.Questing();

                }

            }
            enemyCurrentHealth = null;
        }
    }



    public void FireProjectile()
    {
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = transform.forward * bulletSpeed;
        }
    }
    private void DestroyProjectile()
    {
        float timer = 5;

        if (timeToDestroy >= timer)
        {
            timeToDestroy = 0;

            ReturnBulletToObject();

        }

    }


    private void ReturnBulletToObject()
    {
        BulletPool.Instance.ReturnBullet(bulletPrefab, gameObject);
    }


}
