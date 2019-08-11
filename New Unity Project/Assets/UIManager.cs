using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SimpleSingletonMono<UIManager> {
    UI_Subtitle m_SubTitle,m_Tips;
    UI_Helps m_Helps;
    UI_Dialog m_Dialog;
    protected override void Awake()
    {
        base.Awake();
        m_SubTitle = transform.Find("UI_Subtitle").GetComponent<UI_Subtitle>();
        m_Tips = transform.Find("UI_Tips").GetComponent<UI_Subtitle>();
        m_Helps = transform.Find("UI_Helps").GetComponent<UI_Helps>();
        m_Dialog = transform.Find("UI_Dialog").GetComponent<UI_Dialog>();
    }
    public void AddSubtitle(string subTitle)
    {
        m_SubTitle.AddSubtitle(subTitle);
    }
    public void AddTips(string tips)
    {
        m_Tips.AddSubtitle(tips);
    }

    public void SwitchHelpsShow()
    {
        m_Helps.SwitchShow();
    }
    public void ShowDialog(string main,string[] dialogs,Action<int> OnDialogInteracted)
    {
        m_Dialog.Show(main,dialogs, OnDialogInteracted);
    }
}
