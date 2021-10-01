using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AStarPathFinding : MonoBehaviour
{
    AStarPathRequestManager requestManager;

    MyGrid grid;

    void Awake(){
        requestManager = GetComponent<AStarPathRequestManager>();
        grid = GetComponent<MyGrid>();
    }



    public void StartFindPath(Vector3 startPos, Vector3 targetPos){
        StartCoroutine(FindPath(startPos,targetPos));
        //Debug.Log("111--StartFindPath");

    }

    private float getNodesDistance(Node NodeA, Node NodeB){
        return Vector3.Distance(NodeA.worldPosition,NodeB.worldPosition);
    }

    Vector3[] OutputPath(Node start, Node end){
        List<Node> path = new List<Node>();
        Node current = end;

        while (current!= start){
            path.Add(current);
            current = current.parent;
        }
        
        Vector3[] pathPoints= SimplifyPathByDirection(path);
        Array.Reverse(pathPoints);
        //Debug.Log("OutputPath");
        return pathPoints;
    }

    Vector3[] SimplifyPathByDirection(List<Node> path){
        List<Vector3> waypoints = new List<Vector3>();
        Vector3 directionOld= Vector3.zero;

        for (int i=1; i<path.Count;i++){
            Vector3 directionNew= new Vector3(path[i-1].gridX-path[i].gridX, path[i-1].gridY-path[i].gridY,path[i-1].gridZ-path[i].gridZ);
            if (directionNew!=directionOld){
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        //Debug.Log("222---FindPath");

        Vector3[] wayPoints= new Vector3[0];
        bool pathSucess = false;

        Node startNode = grid.NodeFromWorldPosition(startPos);
        Node targetNode = grid.NodeFromWorldPosition(targetPos);
        
        if (startNode.walkable && targetNode.walkable){
            MyHeap<Node> openList = new MyHeap<Node>(grid.MaxSize);
            HashSet<Node> closedList = new HashSet<Node>();
            openList.Add(startNode);
    
            while (openList.Count>0) 
            {
                Node currentNode = openList.RemoveFirst();    //open队列里找可能离终点最近的点
                
            
                closedList.Add(currentNode);

                if (currentNode == targetNode)
                {
                    //sw.Stop();
                    //print("Path found in :"+sw.ElapsedMilliseconds +"ms");
                    //Debug.Log("333---FoundPath");
                    pathSucess= true;
                    break;
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
                        else
                        {
                            openList.UpdateItem(neighbour);
                        }

                    }

                }

            }
        }
        yield return null;

        if (pathSucess){
            wayPoints = OutputPath(startNode,targetNode);
        }
        else{
            //Debug.Log("333---Cannot Find Path");
        }
        requestManager.FinishedProcessingPath(wayPoints,pathSucess);
    }
}
