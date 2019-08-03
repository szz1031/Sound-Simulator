
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteractDoor : InteractorBase,ISingleCoroutine {
    protected HitCheckDynamic[] m_HitCheck;
    protected Animation m_Animation;
    protected Transform tf_Axis, tf_handle;
    public string KeyAudioName;
    public bool b_Opening { get; private set; } = false;
    public bool b_Opened { get; private set; } = false;
    string m_clipName;
    protected override void Awake()
    {
        base.Awake();
        m_HitCheck = GetComponentsInChildren<HitCheckDynamic>();
        m_HitCheck.Traversal((HitCheckDynamic hitcheck) => { hitcheck.Attach(TryInteract); });
        m_Animation = GetComponent<Animation>();
        tf_Axis = transform.Find("Joint/Axis");
        tf_handle = transform.Find("Joint/Body/Handle");
        m_clipName = GetFistClip().name;
    }
    public override bool TryInteract()
    {
        if (b_Opening)
            return false;
        b_Opening = true;
        string subName = b_Opened ? "Close" : "Open";
        m_Audios.Traversal((AudioBase audio) => { audio.Play(subName); });
        m_Animation[m_clipName].normalizedTime = b_Opened ? .4f : 0;
        m_Animation[m_clipName].speed = b_Opened ? -1 : 1;
        m_Animation.Play(m_clipName);
        this.StartSingleCoroutine(0,TIEnumerators.PauseDel(2f,()=>
        {
            b_Opening = false;
        }));
        return true;
    }
    protected void OnKeyAnim()
    {
        if (b_Opened)
            AudioManager.Play(KeyAudioName,gameObject);
        b_Opened = !b_Opened;
    }
    AnimationState GetFistClip()
    {
        foreach (AnimationState state in m_Animation)
            return state;
        return null;
    }
}