using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;
public class InteractStoryItem : InteractItemBase {

    public string S_InteractTips,E_InInteractableTips, S_InteractSubtitle,S_AudioKey;
    public bool B_Interactable { get; protected set; } = true;
    public bool B_Interacted { get; protected set; } = false;
    private void Start()
    {
        TBroadCaster<enum_BC_Game>.Add<enum_Stage>(enum_BC_Game.OnStageStart, OnStageStart);
    }
    private void OnDestroy()
    {
        TBroadCaster<enum_BC_Game>.Remove<enum_Stage>(enum_BC_Game.OnStageStart, OnStageStart);
    }
    protected virtual void OnStageStart(enum_Stage stage)
    {

    }
    public override void  TryInteract()
    {
        if (!B_Interactable)
        {
            if (E_InInteractableTips != null)
                UIManager.Instance.AddTips(E_InInteractableTips);
            return;
        }

        B_Interacted = true;
        if(S_InteractTips!="")
        UIManager.Instance.AddTips(S_InteractTips);

        if(S_InteractSubtitle!="")
        UIManager.Instance.AddSubtitle(S_InteractSubtitle);

        if(S_AudioKey!="")
        AudioManager.Play(S_AudioKey,this.gameObject);

    }
}
