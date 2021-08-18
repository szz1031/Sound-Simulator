using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;
public class AudioManager : SimpleSingletonMono<AudioManager> {
    public AK.Wwise.Event Ww_FootStep;
    public AK.Wwise.Switch Ww_FootStepSwitchConcrete, Ww_FootStepSwitchCarpet, Ww_FootStepSwitchStair, Ww_FootStepSwitchFloor;
    public List<GameObject> Using3DPlayer = new List<GameObject>();
    public List<GameObject> UnUsed3DPlayer = new List<GameObject>();


    public static void PostEvent(string eventName,GameObject obj)
    {
        AkSoundEngine.PostEvent(eventName,obj);
    }
    public static void PlayFootStep(enum_GroundMaterialType mat, GameObject obj)
    {
        switch (mat)
        {
            case enum_GroundMaterialType.Concrete:
                Instance.Ww_FootStepSwitchConcrete.SetValue(obj);
                break;
            case enum_GroundMaterialType.Floor:
                Instance.Ww_FootStepSwitchFloor.SetValue(obj);
                break;
            case enum_GroundMaterialType.Carpet:
                Instance.Ww_FootStepSwitchCarpet.SetValue(obj);
                break;
            case enum_GroundMaterialType.Stair:
                Instance.Ww_FootStepSwitchStair.SetValue(obj);
                break;
        }
        Instance.Ww_FootStep.Post(obj);
    }
    public static void SwitchGameStatus(bool searchMode)
    {
        if (!searchMode)
        {
            AkSoundEngine.SetState("WorldState", "A");
        }
        else
        {
            AkSoundEngine.SetState("WorldState", "B");
        }
    }

    public static void PostEvent2021(string eventName,GameObject obj){
        SoundUnit mSoundUnit = obj.GetComponent<SoundUnit>();

        if (mSoundUnit==null){
            AkSoundEngine.PostEvent(eventName,obj);
        }
        else
        {
            mSoundUnit.PlaySound2021(eventName,true,true);
        }
    }

    public static GameObject Get3DPlayerAtLocation(Vector3 in_position){

        if (UnUsed3DPlayer.Count<=0){
            GameObject newObject = new GameObject("3DPlayer");
            newObject.AddComponent<Audio3DPlayer>();
            newObject.transform.SetPositionAndRotation(in_position,newObject.transform.rotation);
            Using3DPlayer.Add(newObject);
            return newObject;
        }
        else{   
            GameObject oldObject = UnUsed3DPlayer[0];
            oldObject.transform.SetPositionAndRotation(in_position,oldObject.transform.rotation);
            UnUsed3DPlayer.Remove(UnUsed3DPlayer[0]);
            Using3DPlayer.Add(oldObject);
            return oldObject;
        }

    }

    public static void StopAndRecycle3DPlayer(GameObject in_3DPlayer){
        Audio3DPlayer player = in_3DPlayer.GetComponent<Audio3DPlayer>();
        if (player!=null){
            player.StopPlayingSound(1.5f);
        Using3DPlayer.Remove(in_3DPlayer);
        UnUsed3DPlayer.Add(in_3DPlayer);
        }
        else{
            Debug.Log("Cannot Get Audio3DPlayer Component")
        }


    }

    public static void PlaySoundOn3DPlayer(GameObject in_3DPlayer, string in_eventName){
        Audio3DPlayer player = in_3DPlayer.GetComponent<Audio3DPlayer>()
        if (in_eventName!=null&& player!=null){
            AkSoundEngine.PostEvent(in_eventName,in_3DPlayer)
        }

    }


}