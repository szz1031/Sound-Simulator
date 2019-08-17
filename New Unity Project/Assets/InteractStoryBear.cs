using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractStoryBear : InteractStoryItem {
    public override void TryInteract()
    {
        base.TryInteract();
        GameManager.Instance.BearInteract();
    }
}
