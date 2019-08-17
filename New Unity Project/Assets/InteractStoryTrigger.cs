using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractStoryTrigger : InteractStoryItem {

    private void OnTriggerEnter(Collider other)
    {
        if (B_Interacted)
            return;
        base.TryInteract();
        GameManager.Instance.OnStagePush( GameSetting.enum_Stage.Stage2);
    }

}
