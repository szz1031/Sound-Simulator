using System;
using System.Collections;
using System.Collections.Generic;
using GameSetting;
using UnityEngine;

public class InteractStoryLaptop : InteractStoryItem,ISingleCoroutine {
    protected override void OnStageStart(enum_Stage stage)
    {
        base.OnStageStart(stage);
        switch (stage)
        {
            case enum_Stage.Stage2:
                AudioManager.Play("Laptop_Plot_01",gameObject);
                UIManager.Instance.AddSubtitle("Laptop_Plot_01");
                break;
            case enum_Stage.Stage4:
                AudioManager.Play("Laptop_Plot_06", gameObject);
                UIManager.Instance.AddSubtitle("Laptop_Plot_06");
                break;

        }
    }
    public override void TryInteract()
    {
        switch(GameManager.Instance.m_CurrentStage)
        {
            case enum_Stage.Stage2:
                AudioManager.Play("Laptop_Plot_02", gameObject);
                UIManager.Instance.AddSubtitle("Laptop_Plot_02");
                GameManager.Instance.OnStagePush( enum_Stage.Stage3);
                break;

            case enum_Stage.Stage3:
                if (GameManager.Instance.B_BearInteracted){
                    this.StartSingleCoroutine(0,TIEnumerators.PauseDel(1f,()=> {
                        AudioManager.Play("Laptop_Plot_03", gameObject);
                        UIManager.Instance.AddSubtitle("Laptop_Plot_03");
                    }));
                }
                else if (GameManager.Instance.B_RemoteInteracted){
                    AudioManager.Play("Laptop_Plot_04", gameObject);
                    UIManager.Instance.AddSubtitle("Laptop_Plot_04");
                }
                else {
                    AudioManager.Play("Laptop_Plot_05", gameObject);
                    UIManager.Instance.AddSubtitle("Laptop_Plot_05");
                }
              break;
            case enum_Stage.Stage4:
                AudioManager.Play("Laptop_Plot_07", gameObject);
                UIManager.Instance.AddSubtitle("Laptop_Plot_07");
                GameManager.Instance.OnStagePush( enum_Stage.Stage5);
                GameManager.Instance.PickupKey(9);
                break;
        }
        base.TryInteract();
    }
}
