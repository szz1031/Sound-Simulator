using System;
using UnityEngine;
using UnityEngine.UI;
public class UIT_Loading : SimpleSingletonMono<UIT_Loading>,ISingleCoroutine {
    public static void StartLoading(Action OnLoading)
    {
        Instance.Load(OnLoading);
    }
    Image img_Loading;
    Color col_startColor;
     Action OnStartLoading;
    protected override void Awake()
    {
        base.Awake();
        img_Loading = GetComponent<Image>();
        col_startColor = img_Loading.color;
        img_Loading.color = new Color(col_startColor.r, col_startColor.g, col_startColor.b, 0);
        img_Loading.raycastTarget = false;
    }
    void Load(Action _OnStartLoading)
    {
        img_Loading.raycastTarget = true;
        OnStartLoading = _OnStartLoading;
        this.StartSingleCoroutine(0,TIEnumerators.ChangeValueTo(OnValueChanged,0,1,.5f, StartLoading));
    }
    void OnValueChanged(float value)
    {
        img_Loading.color = new Color(col_startColor.r, col_startColor.g, col_startColor.b,value);
    }
    void StartLoading()
    {
        OnStartLoading();
        this.StartSingleCoroutine(0, TIEnumerators.ChangeValueTo(OnValueChanged, 1, 0, .5f, LoadingFinished));
    }
    void LoadingFinished()
    {
        img_Loading.raycastTarget = false;
    }
    private void OnDestroy()
    {
        this.StopSingleCoroutine(0);
    }
}
