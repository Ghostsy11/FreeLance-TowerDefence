using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] private Vector2Int startPoint;  // Start position (editable in Inspector)
    [SerializeField] private Vector2Int endPoint;    // End position (editable in Inspector)

    private GridManager gridManager;
    private List<Node> path = new List<Node>(); // Stores the found path

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();  // Get reference to GridManager
    }

    private void Start()
    {
        if (GameManager.instance.creaitiveMode == true || GameManager.instance.normalMode == true)
        {
            float xStartPoint = ProceduralGeneration.Instance.StartTilePosition.x;
            float yStartPoint = ProceduralGeneration.Instance.StartTilePosition.y;
            float zStartPoint = ProceduralGeneration.Instance.StartTilePosition.z;


            float xEndtPoint = ProceduralGeneration.Instance.EndTilePosition.x;
            float yEndPoint = ProceduralGeneration.Instance.EndTilePosition.y;
            float zEndPoint = ProceduralGeneration.Instance.EndTilePosition.z;

            startPoint = new Vector2Int((int)xStartPoint, (int)zStartPoint);
            endPoint = new Vector2Int((int)xEndtPoint, (int)zEndPoint);

            FindPath();

        }
        else
        {
            FindPath();

        }

    }

    // 1. BFS Algorithm to Find the Path
    private void FindPath()
    {
        Node startNode = gridManager.GetNode(startPoint);
        Node endNode = gridManager.GetNode(endPoint);

        if (startNode == null || endNode == null)
        {
            Debug.LogWarning("Start or End node is invalid! Start: " + startPoint + ", End: " + endPoint);
            return;
        }
        if (!startNode.isWalkable || !endNode.isWalkable)
        {
            Debug.LogWarning("Start or End node is not walkable!");
            return;
        }
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();

        queue.Enqueue(startNode);
        visited.Add(startNode);
        startNode.parent = null;

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            if (current == endNode)  // Reached the destination
            {
                BuildPath(endNode);
                return;
            }

            foreach (Node neighbor in current.neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    neighbor.parent = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        Debug.LogWarning("No path found!");
    }

    // 1. BFS Algorithm to Find the Path
    private void FindPathCreative()
    {
        Node startNode = gridManager.GetNode(startPoint);
        Node endNode = gridManager.GetNode(endPoint);

        if (startNode == null || endNode == null)
        {
            Debug.LogWarning("Start or End node is invalid! Start: " + startPoint + ", End: " + endPoint);
            return;
        }
        if (!startNode.isWalkable || !endNode.isWalkable)
        {
            Debug.LogWarning("Start or End node is not walkable!");
            return;
        }
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();

        queue.Enqueue(startNode);
        visited.Add(startNode);
        startNode.parent = null;

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            if (current == endNode)  // Reached the destination
            {
                BuildPath(endNode);
                return;
            }

            foreach (Node neighbor in current.neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    neighbor.parent = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        Debug.LogWarning("No path found!");
    }


    // 2. Build Path by Backtracking from End Node
    private void BuildPath(Node endNode)
    {
        path.Clear();
        Node current = endNode;

        while (current != null)
        {
            path.Add(current);
            current = current.parent;
        }

        path.Reverse(); // Reverse so it goes from start → end
    }

    // 3. Function to Get the Path (for Enemy Movement)
    public List<Vector2Int> GetPath()
    {
        List<Vector2Int> pathPositions = new List<Vector2Int>();
        foreach (Node node in path)
        {
            pathPositions.Add(node.nodePosition);
        }
        return pathPositions;
    }
}
