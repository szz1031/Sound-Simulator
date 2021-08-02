using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;

public class SoundUnit : MonoBehaviour
{
    public AkEvent AkEvent;
    public Transform target;
    Transform VSource;
    public bool drawPath;
    public float UpdateTime=0.3f;

    Vector3[] path;

    //int targetIndex;

    int pathIndex;
    float pathLength;
    float directLength;
    Vector3 pathLocation;
    Vector3 virtualLocation;
    float myOcclusion;
    float myObstruction;
    float timer;
    private Occlusion mOcclusion;

    Vector3 virtualtarget;   //用来lerp的


	void Awake(){
        GameObject newObject = new GameObject("Empty");
        VSource = newObject.transform;
        mOcclusion = GetComponent<Occlusion>();
    }
    
    void Start(){
        //AStarPathRequestManager.RequestPath(transform.position,target.position, OnPathFound);
        virtualLocation=transform.position;
        virtualtarget=virtualLocation;
        
        VSource.SetPositionAndRotation(virtualLocation,this.transform.rotation);
    }
    
    void Update(){
        if (Input.GetKeyDown(KeyCode.P)){
            AStarPathRequestManager.RequestPath(transform.position,target.position, OnPathFound);
        }

        timer+=Time.deltaTime;
        
        if (timer>UpdateTime){
            AStarPathRequestManager.RequestPath(transform.position,target.position, OnPathFound);
            timer=0;
        }
        
        virtualLocation = 0.8f*virtualLocation + 0.2f*virtualtarget;
        VSource.SetPositionAndRotation(virtualLocation,this.transform.rotation);
        AkSoundEngine.SetObjectPosition(gameObject,VSource);

        myOcclusion=mOcclusion.OcclusionPercentage;
        AkSoundEngine.SetObjectObstructionAndOcclusion(this.gameObject,target.gameObject,myObstruction,myOcclusion);

    }


    public void OnPathFound(Vector3[] newPath, bool pathSucessful){    //只在这里更新寻路相关的数值 不在update里面做更新
        if (pathSucessful){
            path = newPath;
            //targetIndex=0;
            //StopCoroutine("FollowPath");
            //StartCoroutine("FollowPath");

            if (path.Length>0){
                pathIndex=SimplifyPathByRay();
                //Debug.Log(pathIndex);
                pathLocation=path[pathIndex];
                ChangeObstructionByPath();
                SetVirtualSoundLocation();
            }

        }
        else
        {
            pathLocation=this.transform.position;
            myObstruction=1f;
            Debug.Log("Update myObstruction = " + myObstruction);
            path = null;
            directLength = Vector3.Distance(this.transform.position,target.position);
            pathLength=directLength;
            SetVirtualSoundLocation();
        }
    }

    void SetVirtualSoundLocation(){
        if (pathIndex==0){
            virtualtarget = transform.position;
            //VSource.SetPositionAndRotation(this.transform.position,this.transform.rotation);
            //Debug.Log("111");

        }
        else{
            float targetToMid = Vector3.Distance(pathLocation,target.position);


            virtualtarget = target.position + pathLength/targetToMid*(pathLocation-target.position);
            //Debug.Log("222");
            //VSource.SetPositionAndRotation(virtualLocation,this.transform.rotation);
        }


    }

    void ChangeObstructionByPath(){
        pathLength=0.0f;
        directLength = Vector3.Distance(this.transform.position,target.position);

        pathLength += Vector3.Distance(this.transform.position,path[0]);

        for (int i=0;i<path.Length-1;i++){
            pathLength += Vector3.Distance(path[i],path[i+1]); 
        }
        pathLength += Vector3.Distance(path[path.Length-1],target.position);
        myObstruction = 1 - (directLength/pathLength);
        Debug.Log("Update myObstruction = " + myObstruction);
        

    }

    private int SimplifyPathByRay(){       //将path最后一个点改为可以直达目标的第一个点
        RaycastHit hitInfo;
        //Vector3[] newPath = new Vector3[];
        List<Vector3> newPath = new List<Vector3>(); 
        int closeIndex = -1;

        for (int i=0; i<path.Length-1; i++){    //找到第一个可以直连终点的中间节点，无视之后的路径
        
            Vector3 rayDirection= target.position - path[i];
            float rayDistance = Vector3.Distance(target.position, path[i]);
            bool isHit= Physics.Raycast(path[i], rayDirection, out hitInfo, rayDistance, GameSetting.GameLayer.I_SoundCastAll);   //Raycast

            if (isHit && hitInfo.transform.name!="MainCamera"){
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
            return closeIndex;
        }

        path = newPath.ToArray();

        return closeIndex;
    }

    void OnDrawGizmos() {
		if (path!=null && path.Length>0 && drawPath) {
            Gizmos.DrawSphere(this.transform.position, 0.2f);
			for (int i = 1; i < path.Length; i ++) {           
				Gizmos.color = Color.green;
				Gizmos.DrawCube(path[i], Vector3.one/7);
				Gizmos.DrawLine(path[i-1],path[i]);
			}
            Gizmos.DrawLine(this.transform.position,path[0]);
		}
        Gizmos.color = Color.cyan;
        if (VSource){
            Gizmos.DrawSphere(VSource.position,0.5f);
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
