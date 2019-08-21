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
                UIManager.Instance.AddSubtitle("TV2_Plot_1_Subtitle");
                break;
            case enum_Stage.Stage5:
                AudioManager.PostEvent("TV_2_Plot_2", gameObject);
                UIManager.Instance.AddSubtitle("TV2_Plot_2_Subtitle");
                break;
        }
    }
    public override void TryInteract()
    {
        if (B_Interacted || GameManager.Instance.m_CurrentStage != enum_Stage.Stage5)
            return;
        
        AudioManager.PostEvent("TV_2_Plot_5", gameObject);
        UIManager.Instance.AddSubtitle("TV2_Plot_5_Subtitle");

        base.TryInteract();
    }
}
