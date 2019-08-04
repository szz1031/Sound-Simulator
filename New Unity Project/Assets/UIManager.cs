using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SimpleSingletonMono<UIManager> {
    UI_Subtitle m_SubTitle;
    protected override void Awake()
    {
        base.Awake();
        m_SubTitle = transform.Find("UI_Subtitle").GetComponent<UI_Subtitle>();
    }
    public void AddSubtitle(string subTitle)
    {
        m_SubTitle.AddSubtitle(subTitle);
    }
}
