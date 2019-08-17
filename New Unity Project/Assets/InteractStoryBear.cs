using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractStoryBear : InteractStorySpecial<InteractStoryBear> {
    public override void TryInteract()
    {
        base.TryInteract();
        InteractStoryLaptop.Instance.BearInteract();
    }
}
