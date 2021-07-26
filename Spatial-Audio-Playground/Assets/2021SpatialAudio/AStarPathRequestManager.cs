using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AStarPathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestsQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;
    static AStarPathRequestManager instance;
    AStarPathFinding pathFinding;
    bool isProcessingPath;

    void Awake(){
        instance=this;
        pathFinding = GetComponent<AStarPathFinding>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[],bool> callback){
        PathRequest newRequest = new PathRequest(pathStart,pathEnd,callback);
        instance.pathRequestsQueue.Enqueue(newRequest);
        instance.TryProcessNext();

    }
   
   void TryProcessNext(){
       if (!isProcessingPath && pathRequestsQueue.Count>0){
           currentPathRequest = pathRequestsQueue.Dequeue();
           isProcessingPath = true;
           pathFinding.StartFindPath(currentPathRequest.pathStart,currentPathRequest.pathEnd);
       }
       
   }
   
    public void FinishedProcessingPath(Vector3[] path, bool success){
        Debug.Log("444---FinishedProcessingPath");
        currentPathRequest.callback(path,success);
        isProcessingPath=false;
        TryProcessNext();
    }

   struct PathRequest{
       public Vector3 pathStart;
       public Vector3 pathEnd;
       public Action<Vector3[],bool> callback;

       public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[],bool> _callback){
           pathStart=_start;
           pathEnd=_end;
           callback=_callback;
       }
   }
}
