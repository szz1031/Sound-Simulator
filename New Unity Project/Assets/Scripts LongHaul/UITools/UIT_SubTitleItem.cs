using UnityEngine;
using UnityEngine.UI;
public class UIT_SubTitleItem:UIT_SystemInfoItem,ISingleCoroutine {
    private void OnDisable()
    {
        this.StopSingleCoroutine(0);
    }
    public override void MoveTo(Vector3 rectPos)
    {
        if (gameObject.activeSelf)
            this.StartSingleCoroutine(0,TIEnumerators.RectTransformLerpTo(rectTransform, rectTransform.anchoredPosition, rectPos, .5f));
        else
            rectTransform.anchoredPosition = rectPos;
    }
}
