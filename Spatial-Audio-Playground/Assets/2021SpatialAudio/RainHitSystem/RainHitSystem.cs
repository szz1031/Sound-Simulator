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
    public Transform Player;
    public int N = 4;
    public int Interval = 1;
    public float UpdateTime;
    public bool Draw;

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

        timer+=Time.deltaTime;
        
        if (timer>UpdateTime&&Vector3.Distance(Player.position,lastUsedLocation)>1f){
            
            TryUpdateRainHit();
            lastUsedLocation=Player.position;
            timer=0;
        }
        
    }

    void TryUpdateRainHit(){
        
        UpdateHitMapAndHitArea();

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

                        //set rtpc
                        break;
                    }
                }
            }

            if (!foundInNewArea){
                // stop player and recycle
                //set rtpc =0
                lastHitArea[areaIndexInOldArea].soundPlayer=null;
                lastHitArea[areaIndexInOldArea].isUsing=false;
            }

        }

        for (int i=0;i<newHitArea.Length;i++){
            if (!newHitArea[i].isUsing){
                // 分配player
                if (newHitArea[i].tagName!=null){
                    //set rtpc on player
                    //set switch on player
                    //play audio on player
                }
            }
        }

        lastHitArea=newHitArea;
        lastHitMap=newHitMap;

    }

    void UpdateHitMapAndHitArea(){
        Vector3 zeroPos ;
        zeroPos.x=Player.position.x - (float)(N*Interval);
        zeroPos.y=Player.position.y;
        zeroPos.z=Player.position.z - (float)(N*Interval);

        int zeroX = Mathf.RoundToInt(zeroPos.x / Interval) * Interval;
        int zeroY = Mathf.RoundToInt(zeroPos.z / Interval) * Interval;
        
        List<HitMap> newHitList = new List<HitMap>();

        for (int x=0;x<2*N+1;x++){
            for (int y=0;y<2*N+1;y++){
                int pointX =zeroX+x*Interval;
                int pointY =zeroY+y*Interval;
                int indexInLastMap = PointIndexInTheMap(pointX,pointY,lastHitMap);
                if (indexInLastMap==-1){
                    Vector3 rayStartPoint = new Vector3((float)pointX,(float)(Player.position.y+15),(float)pointY);
                    RaycastHit hitInfo;
                    Physics.Raycast(rayStartPoint, new Vector3(0,-1,0), out hitInfo, 20, GameSetting.GameLayer.I_SoundCastAll);

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
            if (newHitMap[i].color==-1 && !newHitMap[i].hitInfo.transform.CompareTag("Untagged")){
                colorIndex++;
                newHitMap[i].color=colorIndex;  //上色

                //初始化队列和各种参数
                string tempTagName=newHitMap[i].hitInfo.transform.tag;

                List<HitMap> tempAreaList= new List<HitMap>();  //临时区域信息
                tempAreaList.Add(newHitMap[i]);

                List<Vector3> tempLocationList = new List<Vector3>();                 //位置信息
                Vector3 tempFirstLocation =newHitMap[i].hitInfo.transform.position;
                tempLocationList.Add(tempFirstLocation);

                Queue<int> LinearIndexQueue= new Queue<int>();  //含有一个点的队列
                LinearIndexQueue.Enqueue(i);
                

                //染色算法
                while (LinearIndexQueue.Count>0){
                    int n = LinearIndexQueue.Dequeue();
                    int[] neighours= GetNeighbourLinearIndex(n);
                    foreach(int j in neighours){
                        if(newHitMap[j].color==-1){
                            if (newHitMap[j].hitInfo.transform.CompareTag(tempTagName) && Vector3.Distance(newHitMap[j].hitInfo.transform.position,tempFirstLocation)<=3 ){
                                newHitMap[j].color=colorIndex;
                                tempAreaList.Add(newHitMap[j]);
                                tempLocationList.Add(newHitMap[j].hitInfo.transform.position);
                                LinearIndexQueue.Enqueue(j);
                            }
                        }
                    }
                }


                //整理信息更新到新区域
                int[] arrayX= new int[tempAreaList.Length];
                int[] arrayY= new int[tempAreaList.Length];
                Vector3 averageLocation= new Vector3(0f,0f,0f);
                for (int k=0; k<tempAreaList.Length;k++){
                    arrayX[k]=tempAreaList[k].pointX;
                    arrayY[k]=tempAreaList[k].pointY;
                    averageLocation+=tempAreaList[k].hitInfo.transform.position;
                }
                averageLocation=averageLocation/tempAreaList.Length;
                


                HitArea tempHitArea= new HitArea(tempAreaList.Length, false, tempTagName, arrayX, arrayY, averageLocation, null);
                newAreaList.Add(tempHitArea);

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
            Debug.Log("Invalid LinearIndex Calculate In : GetNeighbourLinearIndex");
            return 0;
        }
        return ans;
    }

    int PointIndexInTheMap(int  x, int y, HitMap[] map){

        if (map.Length>0){
            if (map[0].x<= x && x <= map[map.Length-1].x && map[0].y <= y && y <= map[map.Length-1].y){
                int index =  Mathf.RoundToInt((x-map[0].x)/Interval) + Mathf.RoundToInt((y-map[0].y)/Interval) * (2*N +1);
                if (x!=map[index].x || y!=map[index].y){
                    Debug.Log("Wrong Index Calculate In Function:PointIndexInTheMap");
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

}