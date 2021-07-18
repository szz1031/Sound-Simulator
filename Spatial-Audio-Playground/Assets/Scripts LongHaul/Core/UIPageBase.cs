using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPageBase : MonoBehaviour,ISingleCoroutine
{
    public static Type t_curPage;
    protected Image img_Background;
    protected Action<bool> OnInteractFinished;
    protected float f_bgAlphaStart;
    bool b_useAnim;
    public static T ShowPage<T>(Transform parentTransform,bool useAnim) where T:UIPageBase
    {
        if (t_curPage == typeof(T))
            return null;

        t_curPage = typeof(T);
        T tempBase = TResources.Instantiate<T>("UI/Pages/" + typeof(T).ToString(), parentTransform);
        tempBase.Init(useAnim);
        return tempBase;
    }
    protected Button btn_Cancel;
    protected Transform tf_Container;
    protected virtual void Init(bool useAnim)
    {
        b_useAnim = useAnim;
        tf_Container = transform.Find("Container");
        img_Background = transform.Find("Background").GetComponent<Image>();
        f_bgAlphaStart = img_Background.color.a;
        btn_Cancel = tf_Container.Find("BtnCancel").GetComponent<Button>();
        btn_Cancel.onClick.AddListener(OnCancelBtnClick);
        if (useAnim)
            this.StartSingleCoroutine(0, TIEnumerators.ChangeValueTo((float value) => {
                tf_Container.localScale = Vector3.one * value;
                img_Background.color = new Color(img_Background.color.r,img_Background.color.g,img_Background.color.b,value*f_bgAlphaStart);
            }
            , 0f, 1f, .5f));
    }
    protected virtual void OnCancelBtnClick()
    {
        Hide();
    }
    protected void Hide()
    {
        btn_Cancel.enabled = false;
        if (b_useAnim)
            this.StartSingleCoroutine(0, TIEnumerators.ChangeValueTo((float value) => {
                tf_Container.localScale = Vector3.one * value;
                img_Background.color = new Color(img_Background.color.r, img_Background.color.g, img_Background.color.b, value * f_bgAlphaStart);
            }
            , 1f, 0f, .5f, OnHideFinished));
        else
            OnHideFinished();
    }
    void OnHideFinished()
    {
        t_curPage = null;
        OnInteractFinished?.Invoke(false);
        Destroy(this.gameObject);
    }

    protected virtual void OnDestroy()
    {
        this.StopAllSingleCoroutines();
    }
}
