using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;

    public float gCost;
    public float hCost;
    
    public int gridX;
    public int gridY;
    public int gridZ;

    public Node parent;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, int _gridZ)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX=_gridX;
        gridY=_gridY;
        gridZ=_gridZ;
    }

    public float fCost{
        get{
            return gCost + hCost;
        }
    }
}   

