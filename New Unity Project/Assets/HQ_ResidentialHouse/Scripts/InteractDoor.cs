
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteractDoor : InteractorBase {
    protected HitCheckDynamic m_HitCheck;
    protected Animation m_Animation;
    public bool b_Opening { get; private set; }
    public bool b_Opened{ get; private set; }
    public int I_DoorIndex = 1;
    string m_clipName;
    protected override void Awake()
    {
        base.Awake();
        m_HitCheck = GetComponentInChildren<HitCheckDynamic>();
        m_HitCheck.Attach(TryInteract);
        m_Animation = GetComponent<Animation>();
        m_clipName = GetFistClip().name;
    }
    public override bool TryInteract()
    {
        if (b_Opening)
            return false;
        b_Opening = true;
        b_Opened = !b_Opened;

        m_Animation[m_clipName].normalizedTime = b_Opened ? .4f : 0;
        m_Animation[m_clipName].speed = b_Opened ? -1 : 1;
        m_Animation.Play(m_clipName);
        return true;
    }
    protected void OnKeyAnim()
    {
        b_Opening = false;
    }
    AnimationState GetFistClip()
    {
        foreach (AnimationState state in m_Animation)
            return state;
        return null;
    }
}