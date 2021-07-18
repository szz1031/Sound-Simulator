using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGI_DialogButton : UIT_GridItem {
    Action<int> OnTrigger;
    Text m_Text;
    protected override void Init()
    {
        base.Init();
        if (m_Text)
            return;
        GetComponentInChildren<Button>().onClick.AddListener(Trigger);
        m_Text=GetComponentInChildren<Text>();
    }
    public void Show(string innerText,Action<int> _OnTrigger)
    {
        m_Text.text = innerText;
        OnTrigger = _OnTrigger;
    }
    void Trigger()
    {
        OnTrigger(I_Index);
    }
}
