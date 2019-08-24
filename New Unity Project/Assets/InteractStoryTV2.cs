using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;
public class InteractStoryTV2 : InteractStorySpecial<InteractStoryTV2> {
    protected override bool B_IgnoreSearchMode => true;
    protected override void OnStageStart(enum_Stage stage)
    {
        switch (stage)
        {
            case enum_Stage.Stage1:
                AudioManager.PostEvent("TV_2_Plot_1",gameObject);
                UIManager.Instance.AddSubtitle("Press E to interact with Objects, Hold Ctrl to squat, Press Q to switch between normal world and dark world");
                break;
            case enum_Stage.Stage5:
                AudioManager.PostEvent("TV_2_Plot_2", gameObject);
                UIManager.Instance.AddSubtitle("Hey! Did you get the final key?");
                break;
        }
    }
    public override void TryInteract()
    {
        if (B_Interacted || GameManager.Instance.m_CurrentStage != enum_Stage.Stage5)
            return;
        
        AudioManager.PostEvent("TV_2_Plot_5", gameObject);
        UIManager.Instance.AddSubtitle("Congradulations! Now you can open the front door and finish the game. Hope your like this game, and i am very happy to hear some feedback afterwards.");

        base.TryInteract();
    }
}
