using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;
public class InteractStoryTV1 : InteractStorySpecial<InteractStoryTV1> {
    public string DialogTips;
    public string[] Dialogues;


    public override void TryInteract()
    {
        UIManager.Instance.ShowDialog(DialogTips, Dialogues, OnDialogInteracted);
    }
    protected virtual void OnDialogInteracted(int index)
    {
        index++;
        AudioManager.Play("TV_1_Music_"+index.ToString(),this.gameObject);
        UIManager.Instance.AddSubtitle("Playing Music" + index.ToString());
        UIManager.Instance.AddTips("Music Switched");
        base.TryInteract();
    }
    public void RemoteInteract()
    {
        AudioManager.Play("TV_1_SwitchVolume", this.gameObject);
        UIManager.Instance.AddSubtitle("Volume_Up");
    }
}
