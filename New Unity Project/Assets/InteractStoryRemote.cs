using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractStoryRemote : InteractStorySpecial<InteractStoryRemote> {

    public override void TryInteract()
    {
        if (GameManager.Instance.m_CurrentStage != GameSetting.enum_Stage.Stage3&&!B_Interacted)
            return;

        InteractStoryLaptop.Instance.RemoteInteract();
        InteractStoryTV1.Instance.RemoteInteract();
        base.TryInteract();
    }
}
