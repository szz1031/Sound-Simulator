using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviormentManager : SimpleSingletonMono<EnviormentManager> {
    public Transform tf_InteractStoryline { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        tf_InteractStoryline = transform.Find("Interactions/Storyline");
    }
}
