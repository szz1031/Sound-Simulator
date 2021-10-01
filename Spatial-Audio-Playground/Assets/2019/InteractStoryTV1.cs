using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;
public class InteractStoryTV1 : InteractStorySpecial<InteractStoryTV1> {
    public string DialogTips;
    public string[] Dialogues;

    protected override void OnStageStart(enum_Stage stage)
    {
        base.OnStageStart(stage);
        if (stage == enum_Stage.Stage4)
            AudioManager.PostEventOnSoundUnit2021("StopObject", this.gameObject);
    }
    public override void TryInteract()
    {
        UIManager.Instance.ShowDialog(DialogTips, Dialogues, OnDialogInteracted);
    }
    protected virtual void OnDialogInteracted(int index)
    {
        index++;
        //AudioManager.PostEvent("TV_1_Music_"+index.ToString(),this.gameObject);
        AudioManager.PostEventOnSoundUnit2021("TV_1_Music_"+index.ToString(),this.gameObject);
        //UIManager.Instance.AddSubtitle("Playing Music" + index.ToString());
        UIManager.Instance.AddTips("Music Switched");
        base.TryInteract();
    }
    public void RemoteInteract()
    {
        AudioManager.PostEventOnSoundUnit2021("TV_1_SetVolume", this.gameObject);
        UIManager.Instance.AddSubtitle("The Volume is turned up now");
    }
}
