
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteractDoor : InteractItemBase, ISingleCoroutine
{
    protected override bool B_IgnoreSearchMode => true;
    protected AudioBase[] m_Audios;
    protected Animation m_Animation;
    public string MainAudioName = "Door";
    public string LockedSubtitle = "Door Locked";
    public string KeyAudioName;
    public int I_KeyIndex = 0;
    public bool B_InteractOnce = false;
    public bool b_Opening { get; private set; } = false;
    public bool b_Opened { get; private set; } = false;
    bool b_onceInteracted = false;
    string m_clipName;
    public float CountDown;
    public bool b_Counting=false;
    public bool CheatMode = false;
    
    public AkRoomPortal m_Portal;
    protected void Awake()
    {
        m_Audios = GetComponentsInChildren<AudioBase>();
        m_Animation = GetComponent<Animation>();
        m_clipName = GetFistClip().name;
    }

    

    private void Start()
    {
        
        
        if (m_Portal!=null) m_Portal.Close();
    }

    public override void TryInteract()
    {
        base.TryInteract();
        if (!CheatMode && I_KeyIndex > 0 && !GameManager.Instance.B_CanDoorOpen(I_KeyIndex))
        {
            AudioManager.PostEvent(MainAudioName+"_Locked",this.gameObject);
            UIManager.Instance.AddSubtitle(LockedSubtitle);
            UIManager.Instance.AddTips(MainAudioName+" Locked!");
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
        //UIManager.Instance.AddSubtitle(MainAudioName+"_" + subName);
        m_Audios.Traversal((AudioBase audio) => { audio.Play(subName); });
        m_Animation[m_clipName].normalizedTime = b_Opened ? .4f : 0;
        m_Animation[m_clipName].speed = b_Opened ? -1 : 1;
        m_Animation.Play(m_clipName);

        if (m_Portal != null)
        {
            if (b_Opened)
            {
                
                ClosePortalDelayed(0.6f);
                //m_Portal.Close();
               
            }
            else
                m_Portal.Open();
        }

        this.StartSingleCoroutine(0,TIEnumerators.PauseDel(1f,()=>
        {
            b_Opened = !b_Opened;
            b_Opening = false;
        }));
    }
    protected void OnKeyAnim()
    {
        if (b_Opened)
            AudioManager.PostEvent(KeyAudioName,gameObject);
    }
    AnimationState GetFistClip()
    {
        foreach (AnimationState state in m_Animation)
            return state;
        return null;
    }

    
    protected override void Update() 
    {
        base.Update();

       
        if (b_Counting)
        {
            CountDown -= Time.deltaTime;
            if (CountDown < 0)
            {
                m_Portal.Close();
                b_Counting = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            CheatMode = true;
            Debug.Log("CheatMode");
            UIManager.Instance.AddTips("CheatMode on. All doors unlocked");
        }

    }
    

    public void ClosePortalDelayed(float time)
    {
        CountDown = time;
        b_Counting = true;
        
    }
    
}