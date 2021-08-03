using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

struct HitMap{
    public int x;
    public int y;
    public RaycastHit hitInfo;
    public int color;
}

struct HitArea{
    public int size;
    public int[] x;
    public int[] y;
    public Vector3 centreLocation;
    public Transform soundPlayer;
    public bool isUsing;
    public string tagName;


}

public class RainHitSystem : MonoBehaviour
{
    public Transform Player;
    public int N = 4;
    public int Interval = 1;
    public float UpdateTime;

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

                        newHitArea[areaIndexInNewArea].soundPlayer=lastHitArea[areaIndexInOldArea].soundPlayer;
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
        
        for (int x=0;x<2*N+1;x++){
            for (int y=0;y<2*N+1;y++){
                int indexInLastMap = PointIndexInTheMap(zeroX+x*Interval,zeroY+y*Interval,lastHitMap);
                if (indexInLastMap==-1){
                    //raycast
                    //存到新图里 color =-1
                }
                else{
                    //复制到新图里 color =-1
                }
            }
        }
        //新图转换成array

        int colorIndex =-1;

        for (int i=0;i<newHitMap.Length;i++){
             //// 写道这里
        }

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