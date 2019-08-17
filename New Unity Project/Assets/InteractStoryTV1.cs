using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;
public class InteractStoryTV1 : InteractStoryItem {
    public string DialogTips;
    public string[] Dialogues;


    public override void TryInteract()
    {
        UIManager.Instance.ShowDialog(DialogTips, Dialogues, OnDialogInteracted);
    }
    protected virtual void OnDialogInteracted(int index)
    {
        index++;
        AudioManager.Play("TV_1_Music_"+index.ToString(),gameObject);
        UIManager.Instance.AddSubtitle("TV_Switch_Music");
        UIManager.Instance.AddTips("Music Switched");
        base.TryInteract();
    }
}
