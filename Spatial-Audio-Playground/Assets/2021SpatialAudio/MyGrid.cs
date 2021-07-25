using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour
{
    public Transform player;
    public LayerMask unwalkableMask;
    public Vector3 gridWorldSize;
    public float nodeRadius;
    public List<Node> path;
    public bool onlyDrawPath;
    Node[,,] grid;

    Vector3 offsetPosition;


    float nodeDiameter;
    int gridSizeX, gridSizeY, gridSizeZ;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
        CreateGrid();
    }
    
    public int MaxSize{
        get{
            return gridSizeX *gridSizeY*gridSizeZ;
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY, gridSizeZ];
        offsetPosition = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2 - Vector3.forward * gridWorldSize.z / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 worldPosition = offsetPosition + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius) + Vector3.forward * (z * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldPosition, nodeRadius/2,unwalkableMask));
                    grid[x, y, z] = new Node(walkable, worldPosition,x,y,z);
                }
            }
        }
    }

    public List<Node> GetNodeNeighbours(Node node){
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x<=1;x++){
            for (int y = -1; y<=1;y++){
                for (int z = -1; z<=1;z++){
                    if (x==0 && y==0 &&z==0){
                        continue;
                    }

                    if (node.gridX+x>=0 && node.gridX+x < gridSizeX && node.gridY+y>=0 && node.gridY+y< gridSizeY && node.gridZ+z>=0 && node.gridZ+z<gridSizeZ){
                        neighbours.Add(grid[node.gridX+x,node.gridY+y,node.gridZ+z]);
                    }
                    
                }
            }
        }
        return neighbours;
            
    }

    public Node NodeFromWorldPosition(Vector3 worldPosition){

        float deltaX = (worldPosition.x - grid[0,0,0].worldPosition.x) / nodeDiameter;
        //Debug.Log("deltaX = "+ deltaX);
        float deltaY = (worldPosition.y- grid[0,0,0].worldPosition.y) / nodeDiameter;
        //Debug.Log("deltaY = "+ deltaY);
        float deltaZ = (worldPosition.z- grid[0,0,0].worldPosition.z) / nodeDiameter;
        //Debug.Log("deltaZ = "+ deltaZ);

        
        
        int x = Mathf.RoundToInt(deltaX);
        int y = Mathf.RoundToInt(deltaY);
        int z = Mathf.RoundToInt(deltaZ);

        
        x =  Mathf.Clamp(x,0,gridSizeX-1);
        y =  Mathf.Clamp(y,0,gridSizeY-1);
        z =  Mathf.Clamp(z,0,gridSizeZ-1);

        return grid[x,y,z];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, gridWorldSize.z));
        if (grid != null)
        {
            Node playerNode = NodeFromWorldPosition(player.position);
            //Debug.Log("Player =" + player.position.x+","+player.position.y+","+player.position.z +"   Point = "+playerNode.worldPosition.x+","+playerNode.worldPosition.y+","+playerNode.worldPosition.z);
            //Debug.Log("Distance="+ Vector3.Distance(player.position,playerNode.worldPosition));
            
            if (onlyDrawPath){
                Gizmos.color = Color.blue;
                if (path!=null){
                    foreach (Node n in grid){
                        if (path.Contains(n)){
                            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter /2)); 
                        }
                    }                  
                }

            }
            else{           
                foreach (Node n in grid)
                {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    if (playerNode ==n)
                    {
                        Gizmos.color= Color.cyan;
                    }
                    if (path!=null){
                        if (path.Contains(n))
                            Gizmos.color = Color.blue;
                        
                    }

                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter /3));
                }

            }
        }
    }
}
