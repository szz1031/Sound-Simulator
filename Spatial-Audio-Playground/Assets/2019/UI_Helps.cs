using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Helps : MonoBehaviour {
    Text m_Tips;
    Transform tf_Container;
    bool b_show;
    private void Awake()
    {
        tf_Container = transform.Find("Container");
        m_Tips=transform.Find("Tips").GetComponent<Text>();
        SetShow(false);
    }
    public void SwitchShow()
    {
        SetShow(!b_show);
    }
    public void SetShow(bool show)
    {
        b_show = show;
        tf_Container.SetActivate(show);
        m_Tips.text = show ?  "Press TAB To Leave":"Press TAB For Helps";
    }
}
