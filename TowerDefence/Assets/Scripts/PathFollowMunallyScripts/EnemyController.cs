using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    private PathToFollow path;
    private int currentIndex;
    [SerializeField] bool hasReachedCastle;
    void Start()
    {
        path = FindObjectOfType<PathToFollow>();
        Debug.Log(path.points.Length);

    }

    void Update()
    {
        MoveEnemy();
        MoveToNextBlock();
    }

    // First Approach
    #region 
    //private void MoveToNextPoint()
    //{
    //    if (!hasReachedCastle)
    //    {
    //        if (Vector3.Distance(transform.position, path.points[currentIndex].position) <= 0.01f)
    //        {
    //            currentIndex++;
    //            if (currentIndex >= path.points.Length)
    //            {
    //                hasReachedCastle = true;
    //            }
    //        }
    //    }
    //}
    #endregion 

    // Secound Approach
    private void MoveToNextBlock()
    {
        if (hasReachedCastle || Vector3.Distance(transform.position, path.points[currentIndex].position) > 0.1f)
            return;

        hasReachedCastle = ++currentIndex >= path.points.Length;
    }

    private void MoveEnemy()
    {
        if (!hasReachedCastle)
        {
            transform.LookAt(path.points[currentIndex]);
            transform.position = Vector3.MoveTowards(transform.position, path.points[currentIndex].position, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Do something
            return;
        }
    }

}
