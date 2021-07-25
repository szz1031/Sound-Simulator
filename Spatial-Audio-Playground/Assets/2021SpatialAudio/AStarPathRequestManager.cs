using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AStarPathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestsQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;
    static AStarPathRequestManager instance;

    void Awake(){
        instance=this;
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[],bool> callback){
        PathRequest newRequest = new PathRequest(pathStart,pathEnd,callback);
        instance.pathRequestsQueue.Enqueue(newRequest);
        instance.TryProcessNext();

    }
   
   void TryProcessNext(){
       
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
