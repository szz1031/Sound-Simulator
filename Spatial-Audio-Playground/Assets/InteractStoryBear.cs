using System.Collections;
using System.Collections.Generic;
using GameSetting;
using UnityEngine;

public class InteractStoryBear : InteractStorySpecial<InteractStoryBear> {
    protected override void OnStageStart(enum_Stage stage)
    {
        base.OnStageStart(stage);
        if (stage == enum_Stage.Stage4)
            AudioManager.PostEvent("StopObject",this.gameObject);
    }
    public override void TryInteract()
    {
        base.TryInteract();
        InteractStoryLaptop.Instance.BearInteract();
    }
}
