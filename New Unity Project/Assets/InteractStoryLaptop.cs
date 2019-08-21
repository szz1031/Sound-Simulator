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
                UIManager.Instance.AddSubtitle("Laptop_Plot_01");
                break;
            case enum_Stage.Stage4:
                AudioManager.PostEvent("Laptop_Plot_06", gameObject);
                UIManager.Instance.AddSubtitle("Laptop_Plot_06");
                break;

        }
    }
    public override void TryInteract()
    {
        switch (GameManager.Instance.m_CurrentStage)
        {
            case enum_Stage.Stage2:
                AudioManager.PostEvent("Laptop_Plot_02", gameObject);
                UIManager.Instance.AddSubtitle("Laptop_Plot_02");
                GameManager.Instance.OnStagePush( enum_Stage.Stage3);
                break;

            case enum_Stage.Stage3:
                if (b_RemoteInteracted)
                {
                    AudioManager.PostEvent("Laptop_Plot_04", gameObject);
                    UIManager.Instance.AddSubtitle("Laptop_Plot_04");
                }
                else {
                    AudioManager.PostEvent("Laptop_Plot_05", gameObject);
                    UIManager.Instance.AddSubtitle("Laptop_Plot_05");
                }
              break;
            case enum_Stage.Stage4:
                AudioManager.PostEvent("Laptop_Plot_07", gameObject);
                UIManager.Instance.AddSubtitle("Laptop_Plot_07");
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
            UIManager.Instance.AddSubtitle("Laptop_Plot_03");
        }));
    }
    public void RemoteInteract()
    {
        b_RemoteInteracted = true;
    }
}
