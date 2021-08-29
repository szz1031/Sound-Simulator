using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;

public class Audio3DPlayer : MonoBehaviour{
    public List<uint> playingList = new List<uint>();

    public void StopPlayingSound(float in_timeInSecond){
        if (playingList.Count>0){
            foreach (uint i in playingList){
                AkSoundEngine.StopPlayingID(i,Mathf.RoundToInt(in_timeInSecond*1000),AkCurveInterpolation.AkCurveInterpolation_Linear);
            }
        }
    }

    public void StopPlayingSound(){
        if (playingList.Count>0){
            foreach (uint i in playingList){
                AkSoundEngine.StopPlayingID(i,1000,AkCurveInterpolation.AkCurveInterpolation_Linear);
                playingList.Remove(i);
            }
        }
    }

    public bool IsPlaying(){
        if (playingList.Count>0){
            return true;
        }
        else{
            return false;
        }
    }

    public void AddPlayingID(uint in_playingID){
        playingList.Add(in_playingID);
    }

}