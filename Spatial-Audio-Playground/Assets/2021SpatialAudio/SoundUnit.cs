using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundUnit : MonoBehaviour
{
    public AkEvent AkEvent;
    public Transform target;
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

            ChangeSoundPosition();

        }
    }

    void ChangeSoundPosition(){
       


    }

    void OnDrawGizmos() {
		if (path != null) {
			for (int i = 0; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);
				Gizmos.DrawLine(path[i-1],path[i]);
			}
		}
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
