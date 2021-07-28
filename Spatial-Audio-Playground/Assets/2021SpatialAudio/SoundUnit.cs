using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundUnit : MonoBehaviour
{
    public AkEvent AkEvent;
    public Transform target;
    public bool drawPath;
    float speed = 1;
    Vector3[] path;
    Vector3[] soundPath;
    float soundPathLength;
    int targetIndex;

    void Start(){
        //AStarPathRequestManager.RequestPath(transform.position,target.position, OnPathFound);
    }
    
    void Update(){
        if (Input.GetKeyDown(KeyCode.P)){
            AStarPathRequestManager.RequestPath(transform.position,target.position, OnPathFound);
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSucessful){
        if (pathSucessful){
            path = newPath;
            targetIndex=0;
            //StopCoroutine("FollowPath");
            //StartCoroutine("FollowPath");

            SimplifyPathByRay();
            ChangeSoundPosition();

        }
    }

    void ChangeSoundPosition(){
       


    }

    void SimplifyPathByRay(){

    }

    void OnDrawGizmos() {
		if (path!=null && path.Length>0 && drawPath) {
            Gizmos.DrawSphere(path[0], 0.2f);
			for (int i = 1; i < path.Length; i ++) {           
				Gizmos.color = Color.green;
				Gizmos.DrawCube(path[i], Vector3.one/7);
				Gizmos.DrawLine(path[i-1],path[i]);
			}
		}
	}
	

    void OnDestory(){
        path=null;
    }


    // IEnumerator FollowPath(){
    //     Vector3 currentWaypoint = path[0];

    //     while (true){
    //         if (transform.position == currentWaypoint){
    //             targetIndex++;
    //             if (targetIndex>= path.Length){
    //                 yield break;
    //             }
    //             currentWaypoint = path[targetIndex];
    //         }

    //         //transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed*Time.deltaTime);
    //         yield return null;
    //     }
        
    // }

}
