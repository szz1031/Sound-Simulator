using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;
public class AudioManager : SimpleSingletonMono<AudioManager> {
    public AK.Wwise.Event Ww_FootStep;
    public AK.Wwise.Switch Ww_FootStepSwitchConcrete;
    public AK.Wwise.Switch Ww_FootStepSwitchCurtain;
    public AK.Wwise.Switch Ww_FootStepSwitchWoodStairs;
    public static void Play(string clipName,GameObject obj)
    {
        AkSoundEngine.PostEvent(clipName,obj);

        UIManager.Instance.AddSubtitle(clipName);
    }
    public static void PlayFootStep(enum_GroundMaterialType mat, GameObject obj)
    {
        switch (mat)
        {
            case enum_GroundMaterialType.Concrete:
                Instance.Ww_FootStepSwitchConcrete.SetValue(obj);
                break;
            case enum_GroundMaterialType.Curtain:
                Instance.Ww_FootStepSwitchCurtain.SetValue(obj);
                break;
            case enum_GroundMaterialType.WoodStairs:
                Instance.Ww_FootStepSwitchWoodStairs.SetValue(obj);
                break;
        }
        Instance.Ww_FootStep.Post(obj);
    }
}