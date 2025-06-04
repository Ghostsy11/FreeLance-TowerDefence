using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public static ProceduralGeneration Instance;
    [SerializeField] float replacemeantTileProcentage = 0.86f;
    [SerializeField] int width;  // Width of the grid (X-axis)
    [SerializeField] int depth;  // Depth of the grid (Z-axis)
    [SerializeField] GameObject tile; // Normal tile
    [SerializeField] GameObject wallTile; // Wall tile (placed around the grid)
    [SerializeField] GameObject pathStraightTile; // Path tile for straight sections
    [SerializeField] GameObject pathTurnTile; // Path tile for turns
    [SerializeField] List<GameObject> tileToReplace; // List of special tiles that replace normal tiles
    [SerializeField] List<GameObject> grid = new List<GameObject>(); // Stores all grid tiles

    private HashSet<Vector2Int> pathPositions = new HashSet<Vector2Int>(); // Stores positions of path tiles
    public List<WayPoint> fullPath = new List<WayPoint>(); // Stores path waypoints

    // 🔹 Public fields to store start and end tile positions

    [SerializeField] GameObject playerCastle;
    [SerializeField] GameObject enemyCastle;
    public Vector3 StartTilePosition { get; private set; }
    public Vector3 EndTilePosition { get; private set; }

    void Awake()
    {
        Instance = this;

        GenerateGrid();   // Creates the base grid
        GeneratePath();   // Creates the random path
        ReplaceTile();    // Replaces some tiles randomly

        Instantiate(playerCastle, StartTilePosition, playerCastle.transform.rotation, gameObject.transform);
        Instantiate(enemyCastle, EndTilePosition, Quaternion.identity, gameObject.transform);


    }

    // Creates the base grid, adding walls around it
    private void GenerateGrid()
    {
        for (int z = -1; z <= depth; z++)  // Loops over rows (-1 to depth for walls)
        {
            for (int x = -1; x <= width; x++)  // Loops over columns (-1 to width for walls)
            {
                Vector3 position = new Vector3(x, 0, z); // Calculate world position
                GameObject newTile;

                // If the tile is on the boundary, it's a wall
                if (x == -1 || x == width || z == -1 || z == depth)
                {
                    newTile = Instantiate(wallTile, position, Quaternion.identity, gameObject.transform);
                }
                else
                {
                    newTile = Instantiate(tile, position, Quaternion.identity, gameObject.transform);
                    grid.Add(newTile);  // Add normal tile to the grid list
                }
            }
        }
    }

    // Generates a random path from one edge to another
    private void GeneratePath()
    {
        // Pick a random start position on one edge
        Vector2Int start = GetRandomEdgePosition(out Vector2Int direction);

        // Pick the opposite end position on the other side of the grid
        Vector2Int end = GetOppositeEdgePosition(start, direction);

        // 🔹 Store the start and end world positions
        StartTilePosition = new Vector3(start.x, 0, start.y);
        EndTilePosition = new Vector3(end.x, 0, end.y);

        // Create a random but connected path
        pathPositions = GenerateDrunkWalkPath(start, end);

        // Place path tiles in the world
        PlacePathTiles();
    }

    // Generates a random winding path using a "drunk walk" algorithm
    private HashSet<Vector2Int> GenerateDrunkWalkPath(Vector2Int start, Vector2Int end)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>(); // Store path positions
        path.Add(start); // Start position is always part of the path

        Vector2Int current = start; // Start at the beginning
        int maxSteps = width * depth; // Prevent infinite loops

        while (current != end && maxSteps-- > 0) // Stop when we reach the end OR exceed max steps
        {
            List<Vector2Int> possibleMoves = new List<Vector2Int>();

            // Prioritize moves toward the goal
            if (current.x < end.x) possibleMoves.Add(Vector2Int.right);
            if (current.x > end.x) possibleMoves.Add(Vector2Int.left);
            if (current.y < end.y) possibleMoves.Add(Vector2Int.up);
            if (current.y > end.y) possibleMoves.Add(Vector2Int.down);

            // Add random moves for variation
            possibleMoves.Add(Vector2Int.left);
            possibleMoves.Add(Vector2Int.right);
            possibleMoves.Add(Vector2Int.up);
            possibleMoves.Add(Vector2Int.down);

            // Shuffle the moves to add randomness
            possibleMoves = ShuffleList(possibleMoves);

            foreach (Vector2Int move in possibleMoves)
            {
                Vector2Int next = current + move;
                if (IsValidMove(next)) // Check if it's a valid position
                {
                    path.Add(next);
                    current = next; // Move to next tile
                    break;
                }
            }
        }

        return path;
    }

    // Places path tiles into the world
    private void PlacePathTiles()
    {
        fullPath.Clear(); // Clear previous waypoints in case of regeneration

        foreach (Vector2Int pos in pathPositions)
        {
            Vector3 worldPos = new Vector3(pos.x, 0, pos.y);
            GameObject newTile = Instantiate(pathStraightTile, worldPos, Quaternion.identity, gameObject.transform);

            // Ensure the tile has a WayPoint component
            WayPoint waypoint = newTile.GetComponent<WayPoint>();
            if (waypoint == null)
            {
                waypoint = newTile.AddComponent<WayPoint>(); // Add WayPoint if missing
            }

            fullPath.Add(waypoint); // Store WayPoint in the list

            int gridIndex = pos.y * width + pos.x;
            Destroy(grid[gridIndex]);
            grid[gridIndex] = newTile;
        }
    }

    // Checks if the move is within bounds and not already part of the path
    private bool IsValidMove(Vector2Int position)
    {
        return position.x >= 0 && position.x < width &&
               position.y >= 0 && position.y < depth &&
               !pathPositions.Contains(position);
    }

    // Replaces some normal tiles with special terrain tiles
    private void ReplaceTile()
    {
        for (int i = 0; i < grid.Count; i++)
        {
            // 14% chance to replace a tile (but NOT the path tiles)
            if (Random.value > replacemeantTileProcentage && !pathPositions.Contains(new Vector2Int(i % width, i / width)))
            {
                GameObject newTilePrefab = tileToReplace[Random.Range(0, tileToReplace.Count)];
                Vector3 position = grid[i].transform.position;

                Destroy(grid[i]);
                GameObject newTile = Instantiate(newTilePrefab, position, Quaternion.identity, gameObject.transform);

                grid[i] = newTile;
            }
        }
    }

    // Returns a random position on the edge of the grid
    private Vector2Int GetRandomEdgePosition(out Vector2Int direction)
    {
        int edge = Random.Range(0, 4); // Pick a random edge
        switch (edge)
        {
            case 0: direction = Vector2Int.down; return new Vector2Int(Random.Range(0, width), depth - 1); // Top
            case 1: direction = Vector2Int.up; return new Vector2Int(Random.Range(0, width), 0); // Bottom
            case 2: direction = Vector2Int.right; return new Vector2Int(0, Random.Range(0, depth)); // Left
            case 3: direction = Vector2Int.left; return new Vector2Int(width - 1, Random.Range(0, depth)); // Right
            default: direction = Vector2Int.zero; return Vector2Int.zero;
        }
    }

    // Returns the position on the opposite edge of the given start position
    private Vector2Int GetOppositeEdgePosition(Vector2Int start, Vector2Int direction)
    {
        if (direction == Vector2Int.up) return new Vector2Int(start.x, depth - 1);
        if (direction == Vector2Int.down) return new Vector2Int(start.x, 0);
        if (direction == Vector2Int.left) return new Vector2Int(0, start.y);
        if (direction == Vector2Int.right) return new Vector2Int(width - 1, start.y);
        return Vector2Int.zero;
    }

    // Randomly shuffles a list (used to add randomness to movement choices)
    private List<Vector2Int> ShuffleList(List<Vector2Int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Vector2Int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
}
