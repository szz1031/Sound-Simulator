using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour { 
    public Transform Camera;
    public Transform[] Door;
    public bool InRoom;
    public Transform ClosestDoor;
    float MinDistance;
    int Num;
    int count;

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        count++;  
    }

    private void OnTriggerExit(Collider other)
    {
        count--;
    }

    // Update is called once per frame
    void Update ()
    {
        //whether in the room
        if (count == 0)
        {
            InRoom = false;
        //    Debug.Log("EXIT " + name);
        }
        else
        {
            InRoom = true;
        //    Debug.Log("Enter " + name);
        }
            


        //search for the closet door
        if (Door.Length == 0) //only have one door
            ClosestDoor = Door[0];
        else
        {
            Num = 0;
            for (int i = 0; i < Door.Length;i++)  //sort for the cloest door
                if (Vector3.Distance(Door[i].position, Camera.position) < Vector3.Distance(Door[Num].position, Camera.position))
                    Num = i;
            ClosestDoor = Door[Num];
        }
	}
}
