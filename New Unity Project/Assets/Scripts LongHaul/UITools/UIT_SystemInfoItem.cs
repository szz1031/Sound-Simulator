using UnityEngine;
using UnityEngine.UI;
public class UIT_SystemInfoItem:UIT_GridItem {
    Text txt_chatInfo;
    public virtual void MoveTo(Vector3 rectPos)
    {
        rectTransform.anchoredPosition = rectPos;
    }
    protected override void Init()
    {
        if (txt_chatInfo == null)
        {
            txt_chatInfo = GetComponent<Text>();
            rtf_RectTransform = GetComponent<RectTransform>();
        }
    }
    public void SetItem(int itemIndex, string text)
    {
        I_Index = itemIndex;
        txt_chatInfo.text = text;
    }
}
