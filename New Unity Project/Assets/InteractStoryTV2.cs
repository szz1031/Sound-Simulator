using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;
public class InteractStoryTV2 : InteractStoryItem {
    protected override void OnStageStart(enum_Stage stage)
    {
        switch (stage)
        {
            case enum_Stage.Stage1:
                AudioManager.Play("TV_2_Plot_1",gameObject);
                UIManager.Instance.AddSubtitle("TV2_Plot_1_Subtitle");
                break;
            case enum_Stage.Stage5:
                AudioManager.Play("TV_2_Plot_2", gameObject);
                UIManager.Instance.AddSubtitle("TV2_Plot_2_Subtitle");
                break;
        }
    }
}
