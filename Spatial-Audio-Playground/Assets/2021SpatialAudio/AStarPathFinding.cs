using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding : MonoBehaviour
{
    public Transform seeker, target;
    Grid grid;

    void Awake(){
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)){
            FindPath(seeker.position,target.position);
        }
    }

    private float getNodesDistance(Node NodeA, Node NodeB){
        return Vector3.Distance(NodeA.worldPosition,NodeB.worldPosition);
    }

    private void OutputPath(Node start, Node end){
        List<Node> path = new List<Node>();
        Node current = end;

        while (current!= start){
            path.Add(current);
            current = current.parent;
        }
        path.Reverse();
        grid.path = path;
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPosition(startPos);
        Node targetNode = grid.NodeFromWorldPosition(targetPos);

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();
        openList.Add(startNode);
 
        while (openList.Count>0) 
        {
            Node currentNode = openList[0];
            for (int i=1; i< openList.Count;i++)
            {
                if (openList[i].fCost<currentNode.fCost || openList[i].fCost==currentNode.fCost && openList[i].hCost<currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode== targetNode)
            {
                OutputPath(startNode,targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNodeNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedList.Contains(neighbour))   // 跳过这个邻接点
                {
                    continue;
                }
                float newMovementCostToNeighbour = currentNode.gCost + getNodesDistance(currentNode,neighbour);
                if (newMovementCostToNeighbour< neighbour.gCost || !openList.Contains(neighbour)){              //这个点的有更近的路线,则写入新的信息：g h 以及父节点
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = getNodesDistance(neighbour,targetNode);
                    neighbour.parent = currentNode;

                    if (!openList.Contains(neighbour)){
                        openList.Add(neighbour);
                    }

                }

            }

        }

    }
}
