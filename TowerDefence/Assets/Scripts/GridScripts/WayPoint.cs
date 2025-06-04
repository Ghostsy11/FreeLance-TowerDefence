using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] Vector2Int waypointPosition;
    [SerializeField] bool isWalkable;
    public bool isPlacable;
    public Vector3 waypointCurrentLocation;

    private void Start()
    {
        //Debug.Log(GetNodePostion());
        //Debug.Log(IsWalkePath());
        //Debug.Log(IsPlacable());

    }

    public Vector2Int GetNodePostion()
    {
        //waypointPosition.x = Mathf.RoundToInt(gameObject.transform.position.x);
        //waypointPosition.y = Mathf.RoundToInt(gameObject.transform.position.z);

        //return waypointPosition;

        waypointPosition.x = Mathf.FloorToInt(gameObject.transform.position.x); // Use Floor instead of Round
        waypointPosition.y = Mathf.FloorToInt(gameObject.transform.position.z);

        return waypointPosition;
    }

    public Vector3 GetCurrentNodeLocation()
    {
        waypointCurrentLocation.x = gameObject.transform.position.x;
        waypointCurrentLocation.z = transform.position.z;
        return waypointCurrentLocation;
    }

    public bool IsWalkePath()
    {
        return isWalkable;
    }

    public bool IsPlacable()
    {
        return isPlacable;
    }

}
