using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2Int nodePosition { get; private set; }  // Grid position
    public bool isWalkable { get; set; }  // Can units walk here?
    public Node parent { get; set; }  // Used for pathfinding backtracking
    public List<Node> neighbors { get; set; }  // Connected nodes

    // Constructor
    public Node(Vector2Int position, bool isWalkable)
    {
        this.nodePosition = position;
        this.isWalkable = isWalkable;
        this.parent = null;
        this.neighbors = new List<Node>();
    }

}

