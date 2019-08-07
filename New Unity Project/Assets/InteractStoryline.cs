using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;
public class InteractStoryline : InteractorBase {

    public bool B_HideOnAwake;
    public string S_InteractTips, S_InteractSubtitle,S_AudioKey;
    Action<enum_Storyline> OnInteract;
    public bool B_Interactable { get; private set; } = false;
    public bool B_Interacted { get; private set; } = false;
    public enum_Storyline E_Storyline { get; private set; } = enum_Storyline.Invalid;
    public void Init(enum_Storyline storyline,Action<enum_Storyline> _OnInteract)
    {
        gameObject.SetActivate(!B_HideOnAwake);
        E_Storyline = storyline;
        OnInteract = _OnInteract;
        B_Interactable = false;
        B_Interacted = false;
    }
    public void Activate()
    {
        B_Interactable = true;
        gameObject.SetActivate(true);
    }
    public override bool TryInteract()
    {
        if (!B_Interactable|| B_Interacted)
            return false;

        B_Interacted = true;
        UIManager.Instance.AddTips(S_InteractTips);
        UIManager.Instance.AddSubtitle(S_InteractSubtitle);
        AudioManager.Play(S_AudioKey,this.gameObject);
        OnInteract(E_Storyline);
        return base.TryInteract();
    }
}
