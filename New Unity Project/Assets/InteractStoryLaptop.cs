using System;
using System.Collections;
using System.Collections.Generic;
using GameSetting;
using UnityEngine;

public class InteractStoryLaptop : InteractStorySpecial<InteractStoryLaptop>,ISingleCoroutine {

    bool b_bearInteracted = false;
    bool b_RemoteInteracted = false;
    protected override void Awake()
    {
        base.Awake();
        b_bearInteracted = false;
        b_RemoteInteracted = false;
    }

    protected override void OnStageStart(enum_Stage stage)
    {
        base.OnStageStart(stage);
        switch (stage)
        {
            case enum_Stage.Stage2:
                AudioManager.PostEvent("Laptop_Plot_01",gameObject);
                UIManager.Instance.AddSubtitle("“Who's that? Can you come here?”");
                break;
            case enum_Stage.Stage4:
                AudioManager.PostEvent("Laptop_Plot_06", gameObject);
                UIManager.Instance.AddSubtitle("“That's sounds great!”");
                break;

        }
    }
    public override void TryInteract()
    {
        switch (GameManager.Instance.m_CurrentStage)
        {
            case enum_Stage.Stage2:
                AudioManager.PostEvent("Laptop_Plot_02", gameObject);
                UIManager.Instance.AddSubtitle("“I have the key of the main door but i don't want to tell you....unless you play a wonderful music.”");
                GameManager.Instance.OnStagePush( enum_Stage.Stage3);
                break;

            case enum_Stage.Stage3:
                if (b_RemoteInteracted)
                {
                    AudioManager.PostEvent("Laptop_Plot_04", gameObject);
                    UIManager.Instance.AddSubtitle("“I've been tired of listening to music on that TV.”");
                }
                else {
                    AudioManager.PostEvent("Laptop_Plot_05", gameObject);
                    UIManager.Instance.AddSubtitle("“Did you play the music? I can't hear anything.”");
                }
              break;
            case enum_Stage.Stage4:
                AudioManager.PostEvent("Laptop_Plot_07", gameObject);
                UIManager.Instance.AddSubtitle("“I have unlock the safe，you can get the key and go now.”");
                GameManager.Instance.PickupKey(9);
                break;
        }
        base.TryInteract();
    }

    public void BearInteract()
    {
        if (b_bearInteracted||GameManager.Instance.m_CurrentStage!= enum_Stage.Stage3)
            return;

        b_bearInteracted = true;
        this.StartSingleCoroutine(0, TIEnumerators.PauseDel(1f, () => {
            AudioManager.PostEvent("Laptop_Plot_03", gameObject);
            UIManager.Instance.AddSubtitle("“Are you kidding me?”");
        }));
    }
    public void RemoteInteract()
    {
        b_RemoteInteracted = true;
    }
}
