
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteractDoor : InteractorBase,ISingleCoroutine
{
    protected AudioBase[] m_Audios;
    protected Animation m_Animation;
    public string KeyAudioName;
    public int I_KeyIndex = 0;
    public bool b_Opening { get; private set; } = false;
    public bool b_Opened { get; private set; } = false;
    string m_clipName;

    protected override void Awake()
    {
        base.Awake();
        m_Audios = GetComponentsInChildren<AudioBase>();
        m_Animation = GetComponent<Animation>();
        m_clipName = GetFistClip().name;
    }
    public override bool TryInteract()
    {
        if (I_KeyIndex > 0 && !GameManager.Instance.B_CanDoorOpen(I_KeyIndex))
        {
            AudioManager.Play("Door_Locked",this.gameObject);
            UIManager.Instance.AddTips("Door Locked!");
            return false;
        }

        if (b_Opening)
            return false;
        b_Opening = true;
        string subName = b_Opened ? "Close" : "Open";
        m_Audios.Traversal((AudioBase audio) => { audio.Play(subName); });
        m_Animation[m_clipName].normalizedTime = b_Opened ? .4f : 0;
        m_Animation[m_clipName].speed = b_Opened ? -1 : 1;
        m_Animation.Play(m_clipName);
        this.StartSingleCoroutine(0,TIEnumerators.PauseDel(1f,()=>
        {
            b_Opened = !b_Opened;
            b_Opening = false;
        }));
        return true;
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