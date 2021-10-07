using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

struct HitMap{
    public int x;
    public int y;
    public RaycastHit hitInfo;
    public int color;

    public HitMap(int _x, int _y, RaycastHit _hitInfo, int _color){
        x=_x;
        y=_y;
        hitInfo=_hitInfo;
        color=_color;
    }
}

struct HitArea{
    public int size;
    public int[] x;
    public int[] y;
    public Vector3 centreLocation;
    public Transform soundPlayer;
    public bool isUsing;
    public string tagName;

    public HitArea(int _size, bool _isUsing, string _tagName, int[] _x, int[] _y, Vector3 _centreLocation, Transform _soundPlayer){
        size=_size;
        isUsing=_isUsing;
        tagName=_tagName;
        x=_x;
        y=_y;
        centreLocation=_centreLocation;
        soundPlayer=_soundPlayer;
    }
}

public class RainHitSystem : MonoBehaviour
{
    public bool Active=false;
    public bool usePathFinding=false;
    public Transform Player;
    public int N = 4;
    public int Interval = 1;
    public float UpdateTime = 0.3f;
    public bool Draw;
    public bool mDebug=false;


    Vector3 lastUsedLocation;
    float timer=0;

    HitArea[] lastHitArea, newHitArea;
    HitMap[] lastHitMap, newHitMap;




    void Start(){
        lastUsedLocation.x=0f;
        lastUsedLocation.y=0f;
        lastUsedLocation.z=0f;
    }

    void Update(){

        if (Active){

            timer+=Time.deltaTime;
            
            if (timer>UpdateTime&&Vector3.Distance(Player.position,lastUsedLocation)>0.8f){
                
                TryUpdateRainHit();
                lastUsedLocation=Player.position;
                timer=0;
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) this.Active=true;
        if (Input.GetKeyDown(KeyCode.T)) this.Active=false;
    }

    void TryUpdateRainHit(){
        
        UpdateHitMapAndHitArea();

        if (lastHitArea!=null){

            for (int i=0;i<lastHitArea.Length;i++){     //遍历每个旧区域
                bool foundInNewArea = false;
                int areaIndexInOldArea = i;

                for (int j=0;j<lastHitArea[i].size;j++){    //遍历旧区域每个点

                    int indexInNewMap = PointIndexInTheMap(lastHitArea[i].x[j],lastHitArea[i].y[j],newHitMap);
                    if (indexInNewMap!=-1){

                        int areaIndexInNewArea = newHitMap[indexInNewMap].color; 
                        
                        if (!newHitArea[areaIndexInNewArea].isUsing && lastHitArea[areaIndexInOldArea].isUsing){
                            foundInNewArea = true;
                            
                            //搬运
                            newHitArea[areaIndexInNewArea].soundPlayer=lastHitArea[areaIndexInOldArea].soundPlayer;
                            lastHitArea[areaIndexInOldArea].soundPlayer=null;
                            newHitArea[areaIndexInNewArea].isUsing=true;
                            lastHitArea[areaIndexInOldArea].isUsing=false;

                            newHitArea[areaIndexInNewArea].soundPlayer.SetPositionAndRotation(newHitArea[areaIndexInNewArea].centreLocation,this.transform.rotation);

                            AkSoundEngine.SetRTPCValue("RainHit_AreaSize",newHitArea[areaIndexInNewArea].size,newHitArea[areaIndexInNewArea].soundPlayer.gameObject,300);  //set rtpc
                            //Debug.Log("Move");
                            break;
                        }
                    }
                }

                if (!foundInNewArea){   //清除不再使用的老区域的player
                    AudioManager.StopAndRecycle3DPlayer(lastHitArea[areaIndexInOldArea].soundPlayer.gameObject); // stop player and recycle
                    AkSoundEngine.SetRTPCValue("RainHit_AreaSize",0f,lastHitArea[areaIndexInOldArea].soundPlayer.gameObject);  //set rtpc =0
                    lastHitArea[areaIndexInOldArea].soundPlayer=null;
                    lastHitArea[areaIndexInOldArea].isUsing=false;
                    //Debug.Log("Stop an area");
                }

            }

        }

        for (int i=0;i<newHitArea.Length;i++){   //遍历新区域，给未使用的区域配置player并播放
            if (!newHitArea[i].isUsing){
                GameObject newPlayer = AudioManager.Get3DPlayerAtLocation(newHitArea[i].centreLocation,usePathFinding);  // 分配player
                newHitArea[i].soundPlayer=newPlayer.transform;
                newHitArea[i].isUsing=true;

                if (GetSwitchNameFromTagName(newHitArea[i].tagName)!=null){
                    AkSoundEngine.SetRTPCValue("RainHit_AreaSize",newHitArea[i].size,newHitArea[i].soundPlayer.gameObject); //set rtpc on player
                    AkSoundEngine.SetSwitch("RainHit_ObjectType", GetSwitchNameFromTagName(newHitArea[i].tagName),newHitArea[i].soundPlayer.gameObject);    //set switch on player
                    AudioManager.PlaySoundOn3DPlayer(newHitArea[i].soundPlayer.gameObject,"Play_RainHit_Switch");  //play audio on player
                    if (mDebug) Debug.Log("Play an new area:"+GetSwitchNameFromTagName(newHitArea[i].tagName));
                }
            }
        }

        lastHitArea=newHitArea;
        lastHitMap=newHitMap;

        if (mDebug) Debug.Log("RainHit Area Num = "+ newHitArea.Length);

    }

    void UpdateHitMapAndHitArea(){
        Vector3 zeroPos ;
        zeroPos.x=Player.position.x - (float)(N*Interval);
        zeroPos.y=Player.position.y;
        zeroPos.z=Player.position.z - (float)(N*Interval);

        int zeroX = Mathf.RoundToInt(zeroPos.x / Interval) * Interval;
        int zeroY = Mathf.RoundToInt(zeroPos.z / Interval) * Interval;
        
        List<HitMap> newHitList = new List<HitMap>();

        for (int y=0;y<2*N+1;y++){
            for (int x=0;x<2*N+1;x++){
                int pointX =zeroX+x*Interval;
                int pointY =zeroY+y*Interval;
                int indexInLastMap = PointIndexInTheMap(pointX,pointY,lastHitMap);
                if (indexInLastMap==-1){
                    Vector3 rayStartPoint = new Vector3((float)pointX,(float)(Player.position.y+15),(float)pointY);
                    RaycastHit hitInfo;
                    Physics.Raycast(rayStartPoint, new Vector3(0,-20,0), out hitInfo,18f);
                    if (mDebug) Debug.DrawRay(rayStartPoint, new Vector3(0,-20,0),Color.green,UpdateTime);
                    //Debug.Log(hitInfo.transform.name);
                    //Debug.DrawLine(hitInfo.transform.position,hitInfo.transform.position+ new Vector3(0.5f,0,0.5f),Color.red,1.5f);

                    //存到新图里 
                    HitMap temphitmap= new HitMap(pointX,pointY,hitInfo,-1);
                    newHitList.Add(temphitmap);

                }
                else{
                    //复制到新图里 
                    HitMap temphitmap=new HitMap();
                    temphitmap=lastHitMap[indexInLastMap];
                    temphitmap.color=-1;
                    newHitList.Add(temphitmap);

                }
            }
        }
        //得到新图
        newHitMap=newHitList.ToArray();

        int colorIndex =-1;

        List<HitArea> newAreaList = new List<HitArea>();

        //遍历新图
        for (int i=0;i<newHitMap.Length;i++){
            if (newHitMap[i].hitInfo.transform!=null){
                if (newHitMap[i].color==-1 && !newHitMap[i].hitInfo.transform.CompareTag("Untagged")){
                    colorIndex++;
                    newHitMap[i].color=colorIndex;  //上色

                    //初始化队列和各种参数
                    string tempTagName=newHitMap[i].hitInfo.transform.tag;

                    List<HitMap> tempAreaList= new List<HitMap>();  //临时区域信息
                    //tempAreaList.Add(newHitMap[i]);

                    List<Vector3> tempLocationList = new List<Vector3>();                 //位置信息
                    Vector3 tempFirstLocation =newHitMap[i].hitInfo.point;
                    float tempHighestY=newHitMap[i].hitInfo.point.y;
                    //tempLocationList.Add(tempFirstLocation);

                    Queue<int> LinearIndexQueue= new Queue<int>();  //含有一个点的队列
                    LinearIndexQueue.Enqueue(i);
                    

                    //染色算法
                    while (LinearIndexQueue.Count>0){
                        int n = LinearIndexQueue.Dequeue();
                        tempAreaList.Add(newHitMap[n]);
                        tempLocationList.Add(newHitMap[n].hitInfo.point);
                        if (newHitMap[n].hitInfo.point.y>tempHighestY) tempHighestY=newHitMap[n].hitInfo.point.y;
                        int[] neighours= GetNeighbourLinearIndex(n);
                        foreach(int j in neighours){
                            if(newHitMap[j].color==-1&&newHitMap[j].hitInfo.transform!=null){
                                if (newHitMap[j].hitInfo.transform.CompareTag(tempTagName) && Mathf.Abs(tempFirstLocation.y-newHitMap[j].hitInfo.point.y)<=1.5f){
                                    newHitMap[j].color=colorIndex;
                                    //tempAreaList.Add(newHitMap[j]);
                                    //tempLocationList.Add(newHitMap[j].hitInfo.point);
                                    LinearIndexQueue.Enqueue(j);
                                }
                            }
                        }
                    }


                    //整理信息更新到新区域
                    int[] arrayX= new int[tempAreaList.Count];
                    int[] arrayY= new int[tempAreaList.Count];
                    Vector3 averageLocation= new Vector3(0f,0f,0f);
                    for (int k=0; k<tempAreaList.Count;k++){
                        arrayX[k]=tempAreaList[k].x;
                        arrayY[k]=tempAreaList[k].y;
                        averageLocation+=tempAreaList[k].hitInfo.point;
                    }
                    averageLocation=averageLocation/tempAreaList.Count;
                    averageLocation.y=tempHighestY;
                    


                    HitArea tempHitArea= new HitArea(tempAreaList.Count, false, tempTagName, arrayX, arrayY, averageLocation+ new Vector3(0f,0.2f,0f), null);
                    newAreaList.Add(tempHitArea);

                }
            }
        }
        //得到新区域
        newHitArea=newAreaList.ToArray();

    }

    int[] GetNeighbourLinearIndex(int i){
        List<int> NeighbourList= new List<int>();
        int n= 2*N+1;
        int x= i % n;
        int y= i / n;

        if (x>0){
            NeighbourList.Add(GetLinearIndexByXY(x-1,y));
        }

        if (x<2*N){
            NeighbourList.Add(GetLinearIndexByXY(x+1,y));
        }

        if (y>0){
            NeighbourList.Add(GetLinearIndexByXY(x,y-1));
        }

        if (y<2*N){
            NeighbourList.Add(GetLinearIndexByXY(x,y+1));
        }

        return NeighbourList.ToArray();
    }

    int GetLinearIndexByXY(int x, int y){
        int ans = x + y*(2*N+1);
        if (ans < 0 || ans >= newHitMap.Length){
            Debug.Log("Error: Invalid LinearIndex Calculate In : GetNeighbourLinearIndex");
            return 0;
        }
        return ans;
    }

    int PointIndexInTheMap(int  x, int y, HitMap[] map){
        if (map==null){
            if (mDebug) Debug.Log("map=null");
            return -1;
        }

        if (map.Length>0){
            if (map[0].x<= x && x <= map[map.Length-1].x && map[0].y <= y && y <= map[map.Length-1].y){
                int index =  Mathf.RoundToInt((x-map[0].x)/Interval) + Mathf.RoundToInt((y-map[0].y)/Interval) * (2*N +1);
                if (x!=map[index].x || y!=map[index].y){
                    Debug.Log("Error: Wrong Index Calculate In Function:PointIndexInTheMap");
                    return -1;
                }
                return index;
            }
            else{
                return -1;
            }
        }
        else{
            return -1;
        }
    }

    string GetSwitchNameFromTagName(string tagName){
        switch(tagName){
            case null:
                return null;
            case "Grass":
                return "Grass";
            case "Roof":
                return "Tile";
            case "Mud":
                return "WetMud";
            case "Wood":
                return "Wood";
            case "Water":
                return "Water";
            default:
                return null;
        }
    }

    void OnDrawGizmos(){
        if (Draw){
            if (newHitMap!=null){
                foreach(HitMap hitpoint in newHitMap){
                    Gizmos.color=Color.white;
                    Gizmos.DrawCube(hitpoint.hitInfo.point, new Vector3(0.2f,0.1f,0.2f));
                }

            }
            if (newHitArea!=null){
                foreach(HitArea area in newHitArea){
                    Gizmos.color=new Color(0f,0f,0f,0.5f);
                    Gizmos.DrawSphere(area.centreLocation,(float)area.size/20);
                }
            }

        }
    }

}