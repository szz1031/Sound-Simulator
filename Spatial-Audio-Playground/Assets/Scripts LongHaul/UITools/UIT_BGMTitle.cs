using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIT_BGMTitle : SimpleSingletonMono<UIT_BGMTitle>,ISingleCoroutine {
    RectTransform RectTrans;
    Text Title;
    Vector3 startPos;
    Vector3 endPos;
    public float Duration = 2f;
    protected override void Awake()
    {
        base.Awake();
        RectTrans = GetComponent<RectTransform>();
        endPos = RectTrans.anchoredPosition;
        startPos = endPos + Vector3.up*400;
        Title = GetComponentInChildren<Text>();
        RectTrans.anchoredPosition = startPos;
    }
    public void ShowTitle(string text, float duration)
    {
        Title.text = text;
        this.StartSingleCoroutine(0,TIEnumerators.RectTransformLerpTo(RectTrans, startPos, endPos, 2f,  PauseTitle));
    }
    void PauseTitle()
    {
        this.StartSingleCoroutine(0, TIEnumerators.PauseDel(2, HideTitle));
    }
    void HideTitle()
    {
        this.StartSingleCoroutine(0, TIEnumerators.RectTransformLerpTo(RectTrans, endPos, startPos, 2f, null));
    }
    public void OnDestroy()
    {
        this.StopAllSingleCoroutines();
    }
}
