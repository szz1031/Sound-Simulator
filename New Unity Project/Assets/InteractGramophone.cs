using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractGramophone : InteractorBase {
    Animator m_Animator;
    readonly int HS_Start = Animator.StringToHash("Start");
    readonly int HS_Stop = Animator.StringToHash("Stop");
    bool b_playing;
    protected override void Awake()
    {
        base.Awake();
        m_Animator = GetComponent<Animator>();
    }
    public override bool TryInteract()
    {
        base.TryInteract();
        m_Animator.SetTrigger(b_playing?HS_Stop:HS_Start);
        return true;
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
        AudioManager.Play("Gramophone_"+eventName,this.gameObject);
    }
}
