using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractStoryTrigger2 : InteractStoryItem {
    private void OnTriggerEnter(Collider other)
    {
        if (B_Interacted)
            return;
        base.TryInteract();
        UIManager.Instance.AddSubtitle("Game_Finished");
    }
}
