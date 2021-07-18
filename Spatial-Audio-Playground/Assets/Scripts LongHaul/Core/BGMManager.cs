using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceLoader;
using System;

[RequireComponent(typeof(AudioSource))]
public class BGMManager : SimpleSingletonMono<BGMManager>,ISingleCoroutine
{
    public void PlayClip(string clipName)
    {
        this.StartSingleCoroutine(0, TResources.LoadAsync("Audio/Music/"+clipName, (AudioClip clip)=> { audioPlayer.clip = clip;audioPlayer.Play(); }));
    }
    IEnumerator LoadClip(string clipPath,Action<AudioClip> OnClipLoadFinished)
    {
       ResourceRequest request=  Resources.LoadAsync<AudioClip>(clipPath);
        yield return request;
        if (request.isDone && request.asset != null)
            OnClipLoadFinished(request.asset as AudioClip);
    }
    protected AudioSource audioPlayer { get; private set; }
    protected UIT_BGMTitle Title;
    protected override void Awake()
    {
        base.Awake();
        audioPlayer = GetComponent<AudioSource>();
        Init();
    }
    protected virtual void Start()
    {
        Title = UIT_BGMTitle.Instance;
    }
    private void OnDestroy()
    {
        this.StopAllSingleCoroutines();
    }

    protected virtual void Init()
    {
        audioPlayer.loop = false;
        audioPlayer.playOnAwake = false;
        audioPlayer.clip = null;
        audioPlayer.Stop();
    }

    public void PitchClip(float pitch)
    {
        audioPlayer.pitch = audioPlayer.pitch == pitch ? 1f : pitch;
    }
    public AudioSource PlayClip(AudioClip clip)
    {
        audioPlayer.clip = clip;
        audioPlayer.Play();
        if (Title != null)
            Title.ShowTitle(audioPlayer.clip.name, 2f);
        return audioPlayer;
    }
    public void Pause()
    {
        audioPlayer.Pause();
    }
    public void Resume()
    {
        audioPlayer.UnPause();
    }
    public void Stop()
    {
        audioPlayer.Stop();
    }
}
