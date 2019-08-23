using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPath : MonoBehaviour {
    public Transform Camera;
    public Transform Room;
    public Transform VirtualAudioSource;
    public Transform SelectedDoor;
    public bool IsPassFloor;
    public bool IsActive;
    public float MoveAmount;
    public float VolumeDecreased;
    // MoveAmount [0,1] is the most improtant var which determines how much do we want the audio source to move.
    //VolumeDecreased determines how this virtual source being dimed.
    Vector3 Vsource, Vtarget, Vvirtualsource;
    public bool InTheRoom;
    float DistanceToDoor;

	// Use this for initialization
	void Start () {
        VirtualAudioSource.position = transform.position;
        VirtualAudioSource.rotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        IsPassFloor = gameObject.GetComponent<Occlusion>().PassFloor;
        InTheRoom = Room.gameObject.GetComponent<Room>().InRoom;
        SelectedDoor = Room.gameObject.GetComponent<Room>().CloserDoor;

       


        if (!InTheRoom)
        {
            //calculate the MoveAmount
            DistanceToDoor = Vector3.Distance(Camera.position, SelectedDoor.position);

            if (DistanceToDoor >= 1.6f)
                MoveAmount = 1;
            else if (DistanceToDoor <= 0.1f)
                MoveAmount = 0;
            else
                MoveAmount = DistanceToDoor / 1.5f - 0.1f/1.5f;

            //move the audio source position
            Vsource = transform.position - transform.position;
            Vtarget = SelectedDoor.position - transform.position;
            Vvirtualsource = Vector3.Lerp(Vsource, Vtarget, MoveAmount);
            VirtualAudioSource.position = transform.position + Vvirtualsource;

            AkSoundEngine.SetObjectPosition(gameObject, VirtualAudioSource);

            //chage the volume of the audio source
            VolumeDecreased = Vector3.Distance(VirtualAudioSource.position, transform.position) * 0.8f;
            AkSoundEngine.SetRTPCValue("DecreaseVolume", VolumeDecreased, gameObject);
        }
        else
        // reset
        {
            VirtualAudioSource.position = transform.position;
            VirtualAudioSource.rotation = transform.rotation;
            AkSoundEngine.SetObjectPosition(gameObject, transform);
        }
    }
}
