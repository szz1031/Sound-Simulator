
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteractDoor : InteractorBase {
    protected HitCheckDynamic m_HitCheck;
    protected Animation m_Animation;
    protected Transform tf_Axis, tf_handle;
    public bool b_Opening { get; private set; } = false;
    public bool b_Opened { get; private set; } = false;
    public int I_DoorIndex = 1;
    string m_clipName;
    protected override void Awake()
    {
        base.Awake();
        m_HitCheck = GetComponentInChildren<HitCheckDynamic>();
        m_HitCheck.Attach(TryInteract);
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
        AudioManager.Play("Door_"+string.Format("{0:00}", I_DoorIndex)+"_"+(b_Opened?"Close":"Open")+"_PartA",tf_Axis.gameObject);
        AudioManager.Play("Door_" + string.Format("{0:00}", I_DoorIndex) + "_" + (b_Opened ? "Close" : "Open") + "_PartB", tf_handle.gameObject);

        m_Animation[m_clipName].normalizedTime = b_Opened ? .4f : 0;
        m_Animation[m_clipName].speed = b_Opened ? -1 : 1;
        m_Animation.Play(m_clipName);
        return true;
    }
    protected void OnKeyAnim()
    {
        b_Opening = false;
        if (b_Opened)
            AudioManager.Play("Door_01_Close_PartC",gameObject);
        b_Opened = !b_Opened;
    }
    AnimationState GetFistClip()
    {
        foreach (AnimationState state in m_Animation)
            return state;
        return null;
    }
}