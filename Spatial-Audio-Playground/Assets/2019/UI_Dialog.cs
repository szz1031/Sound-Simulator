using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dialog : MonoBehaviour {
    Text m_mainTips;
    Action<int> OnDialogClick;
    UIT_GridControllerMono<UIGI_DialogButton> gc_grid;
    void Awake()
    {
        m_mainTips = transform.Find("MainTips").GetComponent<Text>();
        gc_grid = new UIT_GridControllerMono<UIGI_DialogButton>(transform.Find("Grid"));
        transform.SetActivate(false);
    }
    public void Show(string mainTips,string[] tips,Action<int> onDialogClick)
    {
        m_mainTips.text = mainTips;
        OnDialogClick = onDialogClick;
        gc_grid.ClearGrid();
        for (int i = 0; i < tips.Length; i++)
            gc_grid.AddItem(i).Show(tips[i], OnSelection);

        transform.SetActivate(true);
    }
    public void OnSelection(int selection)
    {
        OnDialogClick(selection);
        transform.SetActivate(false);
    }
}
