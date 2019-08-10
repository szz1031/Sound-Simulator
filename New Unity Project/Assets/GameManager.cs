using GameSetting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SimpleSingletonMono<GameManager> {

    Dictionary<enum_Branch, List<InteractStoryline>> m_Storylines = new Dictionary<enum_Branch, List<InteractStoryline>>();
    private void Start()
    {
        PCInputManager.Instance.AddBinding<GameManager>(enum_BindingsName.Helps, UIManager.Instance.SwitchHelpsShow);
        TCommon.TraversalEnum((enum_Branch value) =>
        {
            List<InteractStoryline> stories = new List<InteractStoryline>();
            Transform storylineParent = EnviormentManager.Instance.tf_Branches.Find(value.ToString());

            for (int i = 0; i < storylineParent.childCount; i++)
            {
                InteractStoryline story = storylineParent.GetChild(i).GetComponent<InteractStoryline>();
                story.Init(value,OnStorylineInteract);
                if (i == 0)
                    story.Activate();

                stories.Add(story);
            }
            m_Storylines.Add(value,stories);
        });
    }
    List<int> m_KeyObtained = new List<int>();
    public bool B_CanDoorOpen(int requireKeyIndex) => m_KeyObtained.Contains(requireKeyIndex);
    void OnStorylineInteract(enum_Branch storyline)
    {
        bool storyAllInteracted = true;
        for (int i = 0; i < m_Storylines[storyline].Count; i++)
        {
            if (!m_Storylines[storyline][i].B_Interacted)
            {
                m_Storylines[storyline][i].Activate();
                storyAllInteracted = false;
                break;
            }
        }

        if (storyAllInteracted)
        {
            Debug.Log("Storyline:" + storyline + ",Complete");
        }
    }
    public void PickupKey(int keyIndex)
    {
        m_KeyObtained.Add(keyIndex);
    }
}
