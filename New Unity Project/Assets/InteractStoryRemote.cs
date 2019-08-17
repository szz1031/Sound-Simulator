using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractStoryRemote : InteractStoryItem {

    public override void TryInteract()
    {
        if (GameManager.Instance.m_CurrentStage != GameSetting.enum_Stage.Stage3&&!B_Interacted)
            return;

        GameManager.Instance.RemoteInteract();
        base.TryInteract();
    }
}
