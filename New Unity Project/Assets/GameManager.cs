using GameSetting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SimpleSingletonMono<GameManager>,ISingleCoroutine {

    public class StoryBranch
    {
        public enum_Branch m_Branch { get; private set; }
        public List<InteractStoryline> m_Items { get; private set; }
        public bool b_Finished { get; private set; }
        public StoryBranch(enum_Branch _branch, List<InteractStoryline> _items)
        {
            m_Branch = _branch;
            m_Items = _items;
            b_Finished = false;
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
    List<StoryBranch> m_Branches=new List<StoryBranch>();
    InteractCharacterLapTop m_LaptopCharacter;
    private void Start()
    {
        PCInputManager.Instance.AddBinding<GameManager>(enum_BindingsName.Helps, UIManager.Instance.SwitchHelpsShow);
        m_LaptopCharacter = EnviormentManager.Instance.tf_Branches.Find("Character_Laptop").GetComponent<InteractCharacterLapTop>();
        TCommon.TraversalEnum((enum_Branch value) =>
        {
            List<InteractStoryline> stories = new List<InteractStoryline>();
            Transform storylineParent = EnviormentManager.Instance.tf_Branches.Find(value.ToString());

            for (int i = 0; i < storylineParent.childCount; i++)
            {
                InteractStoryline story = storylineParent.GetChild(i).GetComponent<InteractStoryline>();
                story.Init(value,OnBranchInteract);
                if (i == 0)
                    story.Activate();

                stories.Add(story);
            }
            m_Branches.Add(new StoryBranch(value,stories));
        });
    }
    List<int> m_KeyObtained = new List<int>();
    public bool B_CanDoorOpen(int requireKeyIndex) =>    m_KeyObtained.Contains(requireKeyIndex);
    public void PickupKey(int keyIndex)=>m_KeyObtained.Add(keyIndex);

    void OnBranchInteract(enum_Branch branch)
    {
        StoryBranch m_Branch = m_Branches.Find(p => p.m_Branch == branch);
        m_Branch.OnBranchInteract();
        
        if(m_Branch.b_Finished)
            OnBranchFinished(branch);
    }

    void OnBranchFinished(enum_Branch branch)
    {
        switch(branch)
        {
            case enum_Branch.BranchBear:
                m_LaptopCharacter.AddOnIntereractOnce(()=> {
                    UIManager.Instance.AddSubtitle("You seem very fond of nursery rhymes, but I don't like it.");
                });
                break;
            case enum_Branch.BranchGuitar:
                this.StartSingleCoroutine(0, TIEnumerators.PauseDel(2f, () => {
                    AudioManager.Play("Character_01_ThatsIt", m_LaptopCharacter.gameObject);
                }));
                m_LaptopCharacter.OnFinished();
                break;
            case enum_Branch.BranchRemote:
                this.StartSingleCoroutine(0, TIEnumerators.PauseDel(2f,()=> {
                    AudioManager.Play("Character_01_ThatsIt",m_LaptopCharacter.gameObject);
                }));
                m_LaptopCharacter.OnFinished();
                break;
        }
    }
}
