using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractGramophone : InteractItemBase {
    Animator m_Animator;
    readonly int HS_Start = Animator.StringToHash("Start");
    readonly int HS_Stop = Animator.StringToHash("Stop");
    bool b_playing;
    protected void Awake()
    {
        m_Animator = GetComponent<Animator>();
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
        AudioManager.Play("Gramophone_"+eventName,this.gameObject);
    }
}
