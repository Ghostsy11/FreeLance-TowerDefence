using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>(); // Stores all nodes in the grid
    [SerializeField] private List<WayPoint> waypoints;  // Assign WayPoints in Inspector

    private void Awake()
    {

        if (GameManager.instance.hardMode)
        {


            GetGridInfo();
            Debug.Log("Grid initialized with " + grid.Count + " nodes.");
        }
    }

    private void Start()
    {
        if (GameManager.instance.creaitiveMode || GameManager.instance.normalMode)
        {

            GetFullPathFromProceduralGeneration();
            GetGridInfo();
        }
    }

    private void GetFullPathFromProceduralGeneration()
    {
        if (ProceduralGeneration.Instance == null)
        {
            Debug.LogError("❌ ProceduralGeneration.Instance is null!");
            return;
        }

        if (ProceduralGeneration.Instance.fullPath == null)
        {
            Debug.LogError("❌ fullPath is null!");
            return;
        }

        if (ProceduralGeneration.Instance.fullPath.Count == 0)
        {
            Debug.LogError("❌ fullPath is empty!");
            return;
        }

        foreach (WayPoint w in ProceduralGeneration.Instance.fullPath)
        {
            if (w == null)
            {
                Debug.LogWarning("⚠ Found null waypoint in fullPath!");
                continue;
            }
            waypoints.Add(w);
        }

        Debug.Log("✅ Loaded " + waypoints.Count + " waypoints from ProceduralGeneration.");
    }

    // 1. Create the grid from WayPoints
    private void GetGridInfo()
    {
        foreach (WayPoint waypoint in waypoints)
        {
            Vector2Int position = waypoint.GetNodePostion();
            bool isWalkable = waypoint.IsWalkePath();

            Debug.Log("Waypoint at: " + position + " | Walkable: " + isWalkable);

            Node newNode = new Node(position, isWalkable);
            grid[position] = newNode;
        }

        // Assign neighbors after all nodes are created
        foreach (Node node in grid.Values)
        {
            node.neighbors = GetNeighbors(node);
            Debug.Log("Node at " + node.nodePosition + " has " + node.neighbors.Count + " neighbors.");

        }
    }

    // 2. Get a node at a specific grid position
    public Node GetNode(Vector2Int position)
    {
        if (grid.ContainsKey(position))
        {
            return grid[position];
        }
        return null; // If no node exists at this position
    }

    // 3. Get walkable neighbors for a node
    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        Vector2Int[] directions =
        {
        new Vector2Int(0, 1),  // Up
        new Vector2Int(1, 0),  // Right
        new Vector2Int(0, -1), // Down
        new Vector2Int(-1, 0)  // Left
    };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = node.nodePosition + dir;

            Debug.Log("Checking neighbor at " + neighborPos + " for node at " + node.nodePosition);

            if (grid.ContainsKey(neighborPos))
            {
                Node neighbor = grid[neighborPos];

                if (neighbor.isWalkable)
                {
                    Debug.Log("✔ Found walkable neighbor at: " + neighborPos);
                    neighbors.Add(neighbor);
                }
            }
        }
        return neighbors;
    }

    // 4. Check if a position is walkable
    public bool IsWalkable(Vector2Int position)
    {
        return grid.ContainsKey(position) && grid[position].isWalkable;
    }

}
