using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIT_JoyStick : SimpleSingletonMono<UIT_JoyStick> {
    RectTransform rtf_Main;
    RectTransform rtf_Center;
    protected override void Awake()
    {
        base.Awake();
        rtf_Main = GetComponent<RectTransform>();
        rtf_Center = transform.Find("Center").GetComponent<RectTransform>();
        rtf_Main.SetActivate(false);
    }
    public void Init(float joyStickRadius)
    {
        rtf_Main.sizeDelta = new Vector2(joyStickRadius*2,joyStickRadius*2);
    }
    public void SetPos(Vector2 startPos,Vector2 centerPos)
    {
        rtf_Main.anchoredPosition = startPos;
        rtf_Center.anchoredPosition = centerPos;
    }
}
