using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;

public class Audio3DPlayer : MonoBehaviour{
    public List<int> playingList = new List<int>();

    public void StopPlayingSound(float in_timeInSecond){
        if (playingList.Count>0){
            foreach (int i in playingList){
                AkSoundEngine.StopPlayingID(i,in_timeInSecond*1000);
            }
        }
    }

    public void StopPlayingSound(){
        if (playingList.Count>0){
            foreach (int i in playingList){
                AkSoundEngine.StopPlayingID(i,1000f);
                playingList.Remove(i);
            }
        }
    }

    public bool IsPlaying(){
        if (playingList.Count>0){
            return true;
        else{
            return false;
        }
    }

}