using System.Collections;
using System.Collections.Generic;
using GameSetting;
using UnityEngine;

public class InteractGramophone : InteractStorySpecial<InteractGramophone> {
    Animator m_Animator;
    readonly int HS_Start = Animator.StringToHash("Start");
    readonly int HS_Stop = Animator.StringToHash("Stop");
    bool b_playing;
    protected override void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }
    protected override void OnStageStart(enum_Stage stage)
    {
        base.OnStageStart(stage);
        if (stage == enum_Stage.Stage4)
            AudioManager.PostEvent("StopObject", this.gameObject);
    }
    public override void TryInteract()
    {
        m_Animator.SetTrigger(b_playing?HS_Stop:HS_Start);
    }
    public void OnEvent(string eventName)
    {
        switch (eventName)
        {
            case "NeedleTap":
                b_playing = true;
                break;
            case "OnOffTap":
                b_playing = false;
                break;
        }
        AudioManager.PostEvent("Gramophone_"+eventName,this.gameObject);
    }
}
