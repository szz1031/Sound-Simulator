using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SimpleSingletonMono<AudioManager> {
    public void Play(InteractorBase interactor, string clipName)
    {
        Debug.Log("Interactor:"+interactor.name+ " Player Clip:" + clipName );
    }
}
