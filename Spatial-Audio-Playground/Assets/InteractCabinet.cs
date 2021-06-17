using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCabinet : InteractItemBase,ISingleCoroutine {
    protected override bool B_IgnoreSearchMode => true;
    protected Animation m_Animation;
    public string MainAudioName="Cabinet";
    public string CloseAudioName="Cabinet_KeySound";
    public bool b_Opening { get; private set; } = false;
    public bool b_Opened { get; private set; } = false;
    string m_clipName;
    protected void Awake()
    {
        m_Animation = GetComponent<Animation>();
        m_clipName = GetFistClip().name;
    }
    public override void TryInteract()
    {
        if (b_Opening)
            return;
        b_Opening = true;
        string name = MainAudioName +  (b_Opened ? "_Close" : "_Open");
        AudioManager.PostEvent(name,this.gameObject);
        //UIManager.Instance.AddSubtitle(name);
        
        m_Animation[m_clipName].normalizedTime = b_Opened ? .4f : 0;
        m_Animation[m_clipName].speed = b_Opened ? -1 : 1;
        m_Animation.Play(m_clipName);
        this.StartSingleCoroutine(0, TIEnumerators.PauseDel(1f, () =>
        {
            b_Opened = !b_Opened;
            b_Opening = false;
        }));
    }
    protected void OnKeyAnim()
    {
        if (b_Opened)
            AudioManager.PostEvent(CloseAudioName, gameObject);
    }
    AnimationState GetFistClip()
    {
        foreach (AnimationState state in m_Animation)
            return state;
        return null;
    }
}
