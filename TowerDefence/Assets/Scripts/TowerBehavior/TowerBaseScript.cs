using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBaseScript : MonoBehaviour
{
    [Header("Range Setteings")]

    public float towerHitRange = 2f;

    [Tooltip("Timer to invoke a fucntion every amount of secound instead of every single frame")]
    [SerializeField] float checkTime = 0.5f;
    private float checkCounter;
    private bool enemiesUpdated;

    [SerializeField] LayerMask whatIsEnemy;
    [SerializeField] List<EnemyHealth> enemyInRage = new List<EnemyHealth>();
    private Collider[] colliderInRange;

    [Tooltip("Holds refrenace to closet target")]
    protected Transform closetTarget;

    [Header("ProjectTile Setteings")]

    [Tooltip("Time between each shot")]
    public float fireRate = 1f;
    private float shotCounter;

    [Tooltip("Buttet prefab is it arrow or cannon?")]
    public GameObject projectile;

    [Tooltip("What is the firing moudle cannon or arrow thrower? and mid point rotation")]
    [SerializeField] Transform lunchingMoudle;

    [Tooltip("Its the exact location where will be the projectile fring from")]
    public Transform firePoint;

    [Tooltip("Its spwan point next to the tower cannon for example to set it ready at beginning of the game")]
    Transform projectileSpwanPointPosition;

    [Header("Range Visuals")]
    public GameObject ringRange;

    private void Start()
    {
        // Initialize the bullet pool for this specific bullet type with a starting size of 10
        if (projectile != null)
        {
            BulletPool.Instance.CreatePool(projectile, 10);

        }
    }
    // Update is called once per frame
    void Update()
    {
        checkCounter += Time.deltaTime;
        shotCounter += Time.deltaTime;

        CheckIfEnemyIsInRange();
        CheckTimerBetweenEveryShot();
        UpdateRingRange();
    }



    /// <summary>
    /// This method changes the point's location to
    /// the given coordinates.
    /// </summary>
    /// <param name="xPosition">the new x-coordinate.</param>
    /// <param name="yPosition">the new y-coordinate.</param>


    #region Enemy Related
    /// <summary>
    /// This method (Checks) if other enemy are in rage eveytime that check timer is complete.
    /// </summary>
    public virtual void CheckIfEnemyIsInRange()
    {
        enemiesUpdated = false;

        if (CheckTimerIsComplete())
        {

            colliderInRange = Physics.OverlapSphere(transform.position, towerHitRange, whatIsEnemy);

            enemyInRage.Clear();

            foreach (Collider collider in colliderInRange)
            {

                enemyInRage.Add(collider.GetComponent<EnemyHealth>());

            }


        }
        enemiesUpdated = true;
        FindTheNearstTarget();
    }

    public virtual void FindTheNearstTarget()
    {
        if (enemiesUpdated)
        {
            if (enemyInRage.Count > 0)
            {
                float minDistance = towerHitRange + 1f;
                foreach (EnemyHealth enemy in enemyInRage)
                {
                    if (enemy != null)
                    {
                        float distance = Vector3.Distance(transform.position, enemy.gameObject.transform.position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closetTarget = enemy.transform;
                            LookAtTheNearstEnemy();
                        }
                    }
                }
            }
            else
            {
                enemiesUpdated = false;
                closetTarget = null;
            }
        }
    }


    public virtual void LookAtTheNearstEnemy()
    {
        if (closetTarget != null)
        {
            //lunchingMoudle.LookAt(closetTarget);
            lunchingMoudle.rotation = Quaternion.Slerp(lunchingMoudle.rotation, Quaternion.LookRotation(closetTarget.position - transform.position), 5f * Time.deltaTime);
            lunchingMoudle.rotation = Quaternion.Euler(0f, lunchingMoudle.rotation.eulerAngles.y, 0f);
            if (CheckTimerBetweenEveryShot())
            {
                ShootAtEnemy();
            }

        }
    }


    public virtual void ShootAtEnemy()
    {
        Debug.Log("Shooting");
        // var bullet = Instantiate(projectile, firePoint.transform.position, firePoint.rotation);

        // Request a bullet from the pool and place it at the fire point's position and rotation
        GameObject bullet = BulletPool.Instance.GetBullet(projectile, firePoint.position, firePoint.rotation);

        if (bullet != null)
        {

            // Initialize the bullet with its prefab reference (used when returning to the pool)
            bullet.GetComponent<Projectile>().Initialize(projectile);
            // Shoot The bullet
            bullet.GetComponent<Projectile>().FireProjectile();
        }

        // Reset time of the firerate
        shotCounter = 0f;



    }

    #endregion

    #region Time Related
    public virtual bool CheckTimerIsComplete()
    {

        if (checkCounter > checkTime)
        {
            checkCounter = 0;
            return true;
        }
        else
        {
            return false;
        }

    }


    public virtual bool CheckTimerBetweenEveryShot()
    {
        // option to add extra check && closetTarget != null
        if (shotCounter >= fireRate)
        {
            shotCounter = 0;
            return true;
        }
        else
        {
            return false;
        }

    }

    #endregion


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, towerHitRange);

        if (ringRange != null)
        {

            float range = towerHitRange * 10;
            ringRange.transform.localScale = new Vector3(range, 0, range);
        }


    }


    private void UpdateRingRange()
    {
        if (ringRange != null)
        {

            float range = towerHitRange * 10;
            ringRange.transform.localScale = new Vector3(range, 0, range);
        }
        else
        {
            return;
        }
    }
}
