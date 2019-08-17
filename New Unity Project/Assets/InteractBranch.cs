using GameSetting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBranch : InteractStoryItem {

    Action<enum_Branch> OnInteract;
    public enum_Branch E_Storyline { get; private set; } = enum_Branch.Invalid;

    public void Init(enum_Branch storyline, Action<enum_Branch> _OnInteract)
    {
        E_Storyline = storyline;
        OnInteract = _OnInteract;
        B_Interactable = false;
        B_Interacted = false;
    }
    public void Activate()
    {
        B_Interactable = true;
    }
    public override void TryInteract()
    {
        if ( B_Interacted)
            return;

        base.TryInteract();
        if(B_Interactable)
        OnInteract(E_Storyline);
    }
}
