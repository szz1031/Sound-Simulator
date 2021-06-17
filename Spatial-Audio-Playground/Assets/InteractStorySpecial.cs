using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractStorySpecial<T> : InteractStoryItem where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        Instance = GetComponent<T>();
    }

}
