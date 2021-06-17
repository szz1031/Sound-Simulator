using GameSetting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SimpleSingletonMono<GameManager>, ISingleCoroutine {

    List<StoryBranch> m_Branches = new List<StoryBranch>();
    List<int> m_KeyObtained = new List<int>();
    public enum_Stage m_CurrentStage;
    public bool B_SearchMode = false;
    protected override void Awake()
    {
        base.Awake();
        TBroadCaster<enum_BC_Game>.Init();
    }
    private void Start()
    {PostEffectManager.AddPostEffect<PE_BSC>();
        B_SearchMode = true;
        SwitchSearchMode();
        PCInputManager.Instance.AddBinding<GameManager>(enum_BindingsName.Helps, UIManager.Instance.SwitchHelpsShow);
        TCommon.TraversalEnum((enum_Branch value) =>
        {
            List<InteractBranch> stories = new List<InteractBranch>();
            Transform storylineParent = EnviormentManager.Instance.tf_Branches.Find(value.ToString());

            for (int i = 0; i < storylineParent.childCount; i++)
            {
                InteractBranch story = storylineParent.GetChild(i).GetComponent<InteractBranch>();
                story.Init(value, OnBranchInteract);
                stories.Add(story);
            }
            StoryBranch branch = new StoryBranch(value, stories);
            branch.Begin();
            m_Branches.Add(branch);
        });
        OnStagePush(enum_Stage.Stage1);
    }
    public void OnStagePush(enum_Stage stage)
    {
        m_CurrentStage = stage;
        TBroadCaster<enum_BC_Game>.Trigger(enum_BC_Game.OnStageStart, m_CurrentStage);
    }
    public bool B_CanDoorOpen(int requireKeyIndex) => m_KeyObtained.Contains(requireKeyIndex);
    public void PickupKey(int keyIndex) 
    {
        m_KeyObtained.Add(keyIndex);
        if (keyIndex == 10)
            OnStagePush(enum_Stage.Stage5);
    } 

    public class StoryBranch
    {
        public enum_Branch m_Branch { get; private set; }
        public List<InteractBranch> m_Items { get; private set; }
        public bool b_Finished { get; private set; }
        public StoryBranch(enum_Branch _branch, List<InteractBranch> _items)
        {
            m_Branch = _branch;
            m_Items = _items;
            b_Finished = false;
        }
        public void Begin()
        {
            m_Items[0].Activate();
        }
        public void OnBranchInteract()
        {
            bool branchComplete = true;

            for (int i = 0; i < m_Items.Count; i++)
            {
                if (!m_Items[i].B_Interacted)
                {
                    m_Items[i].Activate();
                    branchComplete = false;
                    break;
                }
            }
            if (branchComplete)
                b_Finished = true;
        }
    }
    void OnBranchInteract(enum_Branch branch)
    {
        StoryBranch m_Branch = m_Branches.Find(p => p.m_Branch == branch);
        m_Branch.OnBranchInteract();
        
        if(m_Branch.b_Finished)
            OnBranchFinished(branch);
    }

    void OnBranchFinished(enum_Branch branch)
    {
        if (branch == enum_Branch.BranchGuitar)
            OnStagePush( enum_Stage.Stage4);
    }

    public void OnSafeCodeAcquired()
    {
        PickupKey(9);
    }

    public void SwitchSearchMode()
    {
        B_SearchMode = !B_SearchMode;
        AudioManager.SwitchGameStatus(B_SearchMode);
        PE_BSC bsc = PostEffectManager.GetPostEffect<PE_BSC>();
        EnviormentManager.Instance.m_switches.Traversal((LightSwitch ls)=>{ ls.Switch(!B_SearchMode); });
        this.StartSingleCoroutine(0, TIEnumerators.ChangeValueTo((float value) => { bsc.SetEffect(0.6f+value*0.4f, value, .8f+value*0.2f); }, B_SearchMode?1f:0f,B_SearchMode?0f:1f, 2f));
        if (B_SearchMode)
        {

            PostEffectManager.AddPostEffect<PE_DepthOutline>().SetEffect(Color.white,.5f,0f);
            PostEffectManager.AddPostEffect<PE_BloomSpecific>();
        }
        else
        {
            PostEffectManager.RemovePostEffect<PE_DepthOutline>();
            PostEffectManager.RemovePostEffect<PE_BloomSpecific>();

        }
    }
}
