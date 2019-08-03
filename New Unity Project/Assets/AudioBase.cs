using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBase : MonoBehaviour {
    public string AudioName;
    public void Play(string audioNameSub="")
    {
        string jointName = AudioName;
        if (audioNameSub != "")
            jointName += ("_" + audioNameSub);
        AudioManager.Play(jointName, gameObject);
    }
}
