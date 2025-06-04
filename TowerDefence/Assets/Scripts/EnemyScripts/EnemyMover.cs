using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5f;

    private List<Vector2Int> path;
    private PathFinding pathfinder;
    private Coroutine moveCoroutine;

    private void Start()
    {
        pathfinder = FindObjectOfType<PathFinding>();
    }

    private void OnEnable()
    {
        if (pathfinder == null)
        {
            pathfinder = FindObjectOfType<PathFinding>();
        }

        if (moveCoroutine == null) // Make sure only one coroutine runs
        {
            moveCoroutine = StartCoroutine(FollowPath());
        }
    }

    private void OnDisable()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    public IEnumerator FollowPath()
    {
        yield return new WaitUntil(() => pathfinder != null && pathfinder.GetPath().Count > 0);

        while (gameObject.activeSelf)
        {
            path = pathfinder.GetPath();
            if (path == null || path.Count == 0)
            {
                yield return null;
                continue;
            }

            foreach (Vector2Int point in path)
            {
                if (!gameObject.activeSelf) yield break;

                Vector3 targetPos = new Vector3(point.x, transform.position.y, point.y);
                yield return RotateTowards(targetPos);
                yield return MoveToPoint(targetPos);
            }
        }
    }

    private IEnumerator MoveToPoint(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;
        float journeyTime = Vector3.Distance(startPos, targetPos) / moveSpeed;

        while (elapsedTime < journeyTime)
        {
            if (!gameObject.activeSelf) yield break; // Stop if deactivated

            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / journeyTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }

    private IEnumerator RotateTowards(Vector3 targetPos)
    {
        while (true)
        {
            if (!gameObject.activeSelf) yield break; // Stop if deactivated

            Vector3 direction = (targetPos - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
                break;

            yield return null;
        }
    }

}

