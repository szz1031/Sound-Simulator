using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWorld : MonoBehaviour {   
    public int WorldNumber;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (WorldNumber == 0)
        {
            AkSoundEngine.SetState("WorldState", "A");
        }
        else
        {
            AkSoundEngine.SetState("WorldState", "B");
        }

	}
}
