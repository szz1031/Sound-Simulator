using ResourceLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMLoopManager : BGMManager {

    #region Loop
    public enum enum_MusicType
    {
        Invalid = -1,
        Menu,
        Gaming,
        EndCredit,
    }
    public enum enum_PlayType
    {
        Invalid = -1,
        Random,
        InOrder,
        SingleLoop,
    }
    [SerializeField, Range(0, 1)]
    public float F_Volume = 1;
    public bool PlayOnAwake = false;
    public enum_MusicType DefaultPlay = enum_MusicType.Invalid;
    enum_MusicType currentMusicType = enum_MusicType.Invalid;
    enum_PlayType currentPlayType = enum_PlayType.Invalid;
    int currentClipIndex = -1;
    List<AudioClip> currentClips = null;
    public event Action<AudioClip> OnClipPlaying;
    protected override void Start()
    {
        base.Start();
        if (PlayOnAwake)
            SwitchMusicType(DefaultPlay);
    }
    protected override void Init()
    {
        base.Init();
        currentClips = null;
        currentMusicType = enum_MusicType.Invalid;
        currentPlayType = enum_PlayType.Invalid;
    }

    public void SwitchMusicType(enum_MusicType musicType, enum_PlayType type = enum_PlayType.InOrder)
    {
        if (musicType == enum_MusicType.Invalid || type == enum_PlayType.Invalid)
        {
            return;
        }
        currentPlayType = type;
        if (currentMusicType == musicType)
        {
            StartPlaying();
        }
        else
        {
            currentMusicType = musicType;
            this.StartSingleCoroutine(0, ResourcesLoader.LoadAllResourcesAsync<AudioClip>("Audio/Music/" + musicType.ToString(), OnBGMLoadFInished));
        }
    }
    void OnBGMLoadFInished(List<AudioClip> audioClips)
    {
        currentClips = audioClips;
        StartPlaying();
    }
    void StartPlaying()
    {
        currentClipIndex = GetClipIndex(currentClipIndex, currentClips.Count, currentPlayType);
        audioPlayer.clip = currentClips[currentClipIndex];
        audioPlayer.Play();
        OnClipPlaying?.Invoke(currentClips[currentClipIndex]);
        CancelInvoke("Reset");
        CancelInvoke("StartPlaying");
        Invoke("StartPlaying", currentClips[currentClipIndex].length);
        this.StartSingleCoroutine(1, TIEnumerators.ChangeValueTo(delegate (float f) { audioPlayer.volume = f; }, 0, F_Volume, 1f));
        if (Title != null)
            Title.ShowTitle(audioPlayer.clip.name, 2f);
    }
    void StopPlaying()
    {
        CancelInvoke("StartPlaying");
        Invoke("Reset", .5f);
        this.StartSingleCoroutine(1, TIEnumerators.ChangeValueTo(delegate (float f) { audioPlayer.volume = f; }, F_Volume, 0, .5f));
    }
    int GetClipIndex(int fromIndex, int totalClip, enum_PlayType playType)
    {
        int index = -1;
        switch (playType)
        {
            case enum_PlayType.InOrder:
                index = fromIndex == totalClip - 1 ? 0 : fromIndex + 1;
                break;
            case enum_PlayType.Random:
                for (; ; )
                {
                    int toIndex = UnityEngine.Random.Range(0, totalClip);
                    if (toIndex != fromIndex)
                    {
                        index = toIndex;
                        break;
                    }
                }
                break;
            case enum_PlayType.SingleLoop:
                {
                    index = fromIndex == -1 ? UnityEngine.Random.Range(0, totalClip) : fromIndex;
                }
                break;
            default:
                Debug.LogError("Invalid enum_PlayType detected");
                break;
        }
        return index;
    }
    #endregion
}
