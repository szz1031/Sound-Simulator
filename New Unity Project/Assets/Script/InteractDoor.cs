
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteractDoor : InteractItemBase, ISingleCoroutine
{
    protected AudioBase[] m_Audios;
    protected Animation m_Animation;
    public string KeyAudioName;
    public int I_KeyIndex = 0;
    public bool B_InteractOnce = false;
    public bool b_Opening { get; private set; } = false;
    public bool b_Opened { get; private set; } = false;
    bool b_onceInteracted = false;
    string m_clipName;

    protected void Awake()
    {
        m_Audios = GetComponentsInChildren<AudioBase>();
        m_Animation = GetComponent<Animation>();
        m_clipName = GetFistClip().name;
    }
    public override void TryInteract()
    {
        if (I_KeyIndex > 0 && !GameManager.Instance.B_CanDoorOpen(I_KeyIndex))
        {
            AudioManager.Play("Door_Locked",this.gameObject);
            UIManager.Instance.AddSubtitle("Door_Locked");
            UIManager.Instance.AddTips("Door Locked!");
            return;
        }

        if (b_onceInteracted)
            return;
        if (B_InteractOnce)
            b_onceInteracted = true;

        if (b_Opening)
            return;
        b_Opening = true;
        string subName = b_Opened ? "Close" : "Open";
        UIManager.Instance.AddSubtitle("Door " + subName);
        m_Audios.Traversal((AudioBase audio) => { audio.Play(subName); });
        m_Animation[m_clipName].normalizedTime = b_Opened ? .4f : 0;
        m_Animation[m_clipName].speed = b_Opened ? -1 : 1;
        m_Animation.Play(m_clipName);
        this.StartSingleCoroutine(0,TIEnumerators.PauseDel(1f,()=>
        {
            b_Opened = !b_Opened;
            b_Opening = false;
        }));
    }
    protected void OnKeyAnim()
    {
        if (b_Opened)
            AudioManager.Play(KeyAudioName,gameObject);
    }
    AnimationState GetFistClip()
    {
        foreach (AnimationState state in m_Animation)
            return state;
        return null;
    }
}