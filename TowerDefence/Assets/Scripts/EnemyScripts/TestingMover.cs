using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float rotationSpeed = 5f;

    private PathFinding pathfinder;
    private Coroutine moveCoroutine;
    private List<Vector2Int> path = new List<Vector2Int>();

    private void OnEnable()
    {
        pathfinder = FindObjectOfType<PathFinding>();
        StartCoroutine(WaitForPathAndMove());
    }

    private IEnumerator WaitForPathAndMove()
    {
        yield return new WaitUntil(() => pathfinder.GetPath().Count > 0);
        path = pathfinder.GetPath();

        // 🛠️ Reverse the path to make it start at the enemy's position
        path.Reverse();

        Debug.Log("Fixed path order: " + string.Join(" -> ", path));

        if (path.Count > 0)
        {
            moveCoroutine = StartCoroutine(FollowPath());
        }
        else
        {
            Debug.LogWarning("No valid path found.");
        }
    }

    private IEnumerator FollowPath()
    {
        foreach (Vector2Int point in path)
        {
            if (!gameObject.activeSelf) yield break;

            Vector3 targetPos = new Vector3(point.x, transform.position.y, point.y);
            Debug.Log($"Moving to: {targetPos} (Current Pos: {transform.position})");

            yield return RotateTowards(targetPos);
            yield return MoveToPoint(targetPos);
        }
    }

    private IEnumerator MoveToPoint(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos; // Ensure exact position match
    }

    private IEnumerator RotateTowards(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void StopMovement()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
    }
}
