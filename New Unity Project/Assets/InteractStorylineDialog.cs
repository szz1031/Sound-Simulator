using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractStorylineDialog : InteractStoryline {
    public string DialogTips;
    public string[] Dialogues;
    public int I_RightSelections;

    public override void TryInteract()
    {
        if (B_Interacted||!B_Interactable)
            return;
        
        UIManager.Instance.ShowDialog(DialogTips, Dialogues, OnDialogInteracted);
    }
    void OnDialogInteracted(int index)
    {
        if (index == I_RightSelections)
            base.TryInteract();
    }
}
