using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;

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

    int pathIndex;
    Vector3 pathLocation;
    Vector3 virtualLocation;
    float myOcclusion;
    float myObstruction;

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
            //targetIndex=0;
            //StopCoroutine("FollowPath");
            //StartCoroutine("FollowPath");

            if (path.Length>0){
                pathIndex=SimplifyPathByRay();
                pathLocation=path[pathIndex];
                ChangeSoundPosition();
            }

        }
    }

    void ChangeSoundPosition(){
        float pathLength = 0.0f;
        float directLength = Vector3.Distance(this.transform.position,target.position);

        for (int i=0;i<path.Length-1;i++){
            pathLength += Vector3.Distance(path[i],path[i+1]); 
        }
        pathLength += Vector3.Distance(path[path.Length],target.position);

        myObstruction = 1 - (directLength/pathLength);

        AkSoundEngine.SetObjectObstructionAndOcclusion(this.gameObject,target.gameObject,myObstruction,myOcclusion);

    }

    private int SimplifyPathByRay(){       //将path最后一个点改为可以直达目标的第一个点
        RaycastHit hitInfo；
        //Vector3[] newPath = new Vector3[];
        List<Vector3> newPath = new List<Vector3>(); 
        int closeIndex = -1;

        for (int i=0; i<path.Length-1; i++){    //找到第一个可以直连终点的中间节点，无视之后的路径
        
            Vector3 rayDirection= target.position - path[i];
            float rayDistance = Vector3.distance(target.position, path[i]);
            bool isHit= Physics.Raycast(path[i], rayDirection, out hitInfo, rayDistance, GameSetting.GameLayer.I_SoundCastAll);   //Raycast

            if (isHit){
                newPath.Add(path[i]);
            }
            else{
                newPath.Add(path[i]);   //找到第一个直达点
                closeIndex = i;
                break;
            }
        
        }

        if (closeIndex==-1){
            closeIndex=path.Length-1;
        }

        path = newPath.ToArray();
        return closeIndex;
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
