using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class AStarPathFinding : MonoBehaviour
{
    public Transform seeker, target;
    public float updateTime;
    float timer;
    MyGrid grid;

    void Awake(){
        grid = GetComponent<MyGrid>();
    }

    void Update()
    {
        timer=timer+ Time.deltaTime;
        if (timer >= updateTime){
            FindPath(seeker.position,target.position);
            timer=0;
        }

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
        //Debug.Log("Path Length = " +path.Count);
        grid.path = path;
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Node startNode = grid.NodeFromWorldPosition(startPos);
        Node targetNode = grid.NodeFromWorldPosition(targetPos);
        bool isFound=false;

        MyHeap<Node> openList = new MyHeap<Node>(grid.MaxSize);
        HashSet<Node> closedList = new HashSet<Node>();
        openList.Add(startNode);
 
        while (openList.Count>0) 
        {
            Node currentNode = openList.RemoveFirst();    //open队列里找可能离终点最近的点
            
          
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                sw.Stop();
                print("Path found in :"+sw.ElapsedMilliseconds +"ms");
                OutputPath(startNode,targetNode);
                //Debug.Log("Finished Path Finding");
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

        if (!isFound){
            //Debug.Log("Path Not Found");
        }

    }
}
