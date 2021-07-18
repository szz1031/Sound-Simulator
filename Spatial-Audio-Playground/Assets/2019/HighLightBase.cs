using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;
public class HighLightBase : MonoBehaviour {
    public List<enum_Stage> m_ShowStages = new List<enum_Stage>() { enum_Stage.Stage1, enum_Stage.Stage2, enum_Stage.Stage3, enum_Stage.Stage4, enum_Stage.Stage5 };
    private void Start()
    {
        TBroadCaster<enum_BC_Game>.Add<enum_Stage>(enum_BC_Game.OnStageStart, OnStageStart);
    }
    private void OnDestroy()
    {

        TBroadCaster<enum_BC_Game>.Remove<enum_Stage>(enum_BC_Game.OnStageStart, OnStageStart);
    }
    void OnStageStart(enum_Stage stage)
    {
        transform.SetActivate(m_ShowStages.Contains(stage));
    }
}
