using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;
public class AudioManager : SimpleSingletonMono<AudioManager> {
    public AK.Wwise.Event Ww_FootStep;
    public AK.Wwise.Switch Ww_FootStepSwitchConcrete, Ww_FootStepSwitchCarpet, Ww_FootStepSwitchStair, Ww_FootStepSwitchFloor;
    public static void Play(string clipName,GameObject obj)
    {
        AkSoundEngine.PostEvent(clipName,obj);
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
}