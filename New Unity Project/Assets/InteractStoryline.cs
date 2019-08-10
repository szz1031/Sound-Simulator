using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;
public class InteractStoryline : InteractItemBase {

    public bool B_HideOnAwake;
    public string S_InteractTips,E_InInteractableTips, S_InteractSubtitle,S_AudioKey;
    Action<enum_Branch> OnInteract;
    public bool B_Interactable { get; private set; } = false;
    public bool B_Interacted { get; private set; } = false;
    public enum_Branch E_Storyline { get; private set; } = enum_Branch.Invalid;
    public void Init(enum_Branch storyline,Action<enum_Branch> _OnInteract)
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
    public override void  TryInteract()
    {
        if (B_Interacted)
            return;

        if (!B_Interactable)
        {
            if (E_InInteractableTips != null)
                UIManager.Instance.AddTips(E_InInteractableTips);
            return;
        }

        B_Interacted = true;
        UIManager.Instance.AddTips(S_InteractTips);
        UIManager.Instance.AddSubtitle(S_InteractSubtitle);
        AudioManager.Play(S_AudioKey,this.gameObject);
        OnInteract(E_Storyline);
    }
}
