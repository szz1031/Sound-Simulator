using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour { 
    public Transform Camera;
    public Transform[] Door;
    public bool InRoom;
    public Transform CloserDoor;
    float MinDistance;
    int Num;

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        InRoom = true;
        Debug.Log("Get in the room" + name);
    }

    private void OnTriggerExit(Collider other)
    {
        InRoom = false;
        Debug.Log("Get out the room" + name);
    }

    // Update is called once per frame
    void Update () {
        if (Door.Length == 0) //only have one door
            CloserDoor = Door[0];
        else
        {
            Num = 0;
            for (int i = 0; i < Door.Length;i++)  //sort for the cloest door
                if (Vector3.Distance(Door[i].position, Camera.position) < Vector3.Distance(Door[Num].position, Camera.position))
                    Num = i;
            CloserDoor = Door[Num];
        }
	}
}
