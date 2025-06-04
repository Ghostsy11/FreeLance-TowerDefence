using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesPool : MonoBehaviour
{
    public static EnemiesPool Instance { get; private set; }

    [SerializeField] private int poolCapacity = 1000;
    [SerializeField] private GameObject[] enemiesType;
    [SerializeField] private float respawnTime = 5f;
    public Transform SpawnPoint;
    [SerializeField] GameObject loc;
    private Queue<GameObject> inactiveEnemies = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (GameManager.instance.creaitiveMode)
        {
            loc = GameObject.FindGameObjectWithTag("EnemySpwanLoc");
            gameObject.transform.position = loc.gameObject.transform.position;
        }

        if (GameManager.instance.normalMode)
        {
            loc = GameObject.FindGameObjectWithTag("EnemySpwanLoc");
            gameObject.transform.position = loc.gameObject.transform.position;
        }

        SpawnEnemiesToPool();
        StartCoroutine(ReactivateEnemies());
    }

    private void SpawnEnemiesToPool()
    {
        for (int i = 0; i < poolCapacity; i++)
        {
            int enemyIndex = Random.Range(0, enemiesType.Length);
            GameObject enemy = Instantiate(enemiesType[enemyIndex], SpawnPoint.position, Quaternion.identity);

            if (!enemy.TryGetComponent(out EnemyMover enemyMover))
            {
                Debug.LogError($"Enemy {enemy.name} is missing EnemyMover component!");
                continue;
            }

            enemy.transform.SetParent(transform);
            enemy.SetActive(false);
            inactiveEnemies.Enqueue(enemy);
        }
    }

    private IEnumerator ReactivateEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);

            if (inactiveEnemies.Count > 0)
            {
                GameObject enemy = inactiveEnemies.Dequeue();
                enemy.transform.position = SpawnPoint.position; // Reset position

                if (enemy.TryGetComponent(out EnemyMover enemyMover))
                {
                    enemy.SetActive(true); // This will trigger OnEnable in EnemyMover
                }
            }
        }
    }

    public void DeactivateEnemy(GameObject enemy)
    {
        if (!inactiveEnemies.Contains(enemy))
        {
            enemy.SetActive(false);

            if (enemy.TryGetComponent(out ObjectHealth health))
            {
                enemy.transform.position = SpawnPoint.position; // Reset Position
                health.ResetHealth();
                health.UpdateHealthBar();
            }

            inactiveEnemies.Enqueue(enemy);
        }
    }
}
