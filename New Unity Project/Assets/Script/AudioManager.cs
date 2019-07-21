using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SimpleSingletonMono<AudioManager> {

    public static void Play(string clipName,GameObject obj)
    {
        AkSoundEngine.PostEvent(clipName,obj);
    }
}


//test