using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;
public class InteractStoryTV1 : InteractStorySpecial<InteractStoryTV1> {
    public string DialogTips;
    public string[] Dialogues;
    public GameObject Target;

    protected override void OnStageStart(enum_Stage stage)
    {
        base.OnStageStart(stage);
        if (stage == enum_Stage.Stage4)
            AudioManager.PostEvent("StopObject", Target);
    }
    public override void TryInteract()
    {
        UIManager.Instance.ShowDialog(DialogTips, Dialogues, OnDialogInteracted);
    }
    protected virtual void OnDialogInteracted(int index)
    {
        index++;
        AudioManager.PostEvent("TV_1_Music_"+index.ToString(),Target);
        //UIManager.Instance.AddSubtitle("Playing Music" + index.ToString());
        UIManager.Instance.AddTips("Music Switched");
        base.TryInteract();
    }
    public void RemoteInteract()
    {
        AudioManager.PostEvent("TV_1_SetVolume", Target);
        UIManager.Instance.AddSubtitle("The Volume is turned up now");
    }
}
