using System.Collections;
using System.Collections.Generic;
using GameSetting;
using UnityEngine;

public class InteractStoryBear : InteractStorySpecial<InteractStoryBear> {
    public GameObject Target;
    protected override void OnStageStart(enum_Stage stage)
    {
        base.OnStageStart(stage);
        if (stage == enum_Stage.Stage4)
            AudioManager.PostEvent("StopObject",Target);
    }
    public override void TryInteract()
    {
        AudioManager.PostEvent("Bear_Music", Target);
        base.TryInteract();
        InteractStoryLaptop.Instance.BearInteract();
    }
}
