using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    // Dictionary storing multiple bullet pools, where each bullet prefab has its own queue of reusable bullets.
    private Dictionary<GameObject, Queue<GameObject>> bulletPools = new Dictionary<GameObject, Queue<GameObject>>();

    // Awake() is called before Start(). Ensures there's only one instance of BulletPool.
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Initialize a pool for a specific bullet type
    // Initializes a pool for a specific bullet type, creating a predefined number of bullets.
    public void CreatePool(GameObject bulletPrefab, int poolSize)
    {
        // If the pool for this bullet type doesn't exist, create it
        if (!bulletPools.ContainsKey(bulletPrefab))
        {
            Queue<GameObject> newPool = new Queue<GameObject>();

            // Instantiate and store inactive bullets in the pool
            for (int i = 0; i < poolSize; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.SetActive(false); // Disable bullets initially
                bullet.transform.SetParent(transform); // Keep hierarchy clean
                newPool.Enqueue(bullet);
            }
            // Store the new pool in the dictionary
            bulletPools[bulletPrefab] = newPool;
        }
    }

    // Get a bullet from the correct pool
    // Retrieves an available bullet from the pool or creates a new one if needed.
    public GameObject GetBullet(GameObject bulletPrefab, Vector3 position, Quaternion rotation)
    {
        // If the pool for this bullet type doesn't exist, create a default-sized one
        if (!bulletPools.ContainsKey(bulletPrefab))
        {
            if (bulletPrefab == null)
            {
                Debug.Log("Its Laser Or Something");
                return null;
            }
            Debug.LogWarning("Pool for this bullet doesn't exist. Creating it now.");
            CreatePool(bulletPrefab, 5); // Default pool size if not initialized

        }

        GameObject bullet;
        // Check if there's an available bullet in the pool
        if (bulletPools[bulletPrefab].Count > 0)
        {
            bullet = bulletPools[bulletPrefab].Dequeue(); // Take a bullet from the pool
        }
        else
        {
            // If pool is empty, instantiate a new bullet
            bullet = Instantiate(bulletPrefab, gameObject.transform); // Expand if needed
        }

        // Set bullet's position and rotation
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.SetActive(true); // Activate the bullet
        return bullet; // Return the bullet for use
    }

    // Return bullet to its respective pool
    // Returns a bullet to the pool so it can be reused instead of being destroyed.
    public void ReturnBullet(GameObject bulletPrefab, GameObject bullet)
    {
        bullet.SetActive(false); // Deactivate the bullet

        // Ensure the bullet's pool exists before returning it
        if (bulletPools.ContainsKey(bulletPrefab))
        {
            bulletPools[bulletPrefab].Enqueue(bullet);  // Add it back to the queue
        }
        else
        {
            // If the pool is missing, destroy the bullet as a fallback
            Destroy(bullet); // Just in case something goes wrong
        }
    }
}
