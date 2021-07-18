using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviormentManager : SimpleSingletonMono<EnviormentManager> {
    public Transform tf_Branches { get; private set; }
    public LightSwitch[] m_switches { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        tf_Branches = transform.Find("Interactions/Branch");
        m_switches = transform.Find("Interactions/LightSwitch").GetComponentsInChildren<LightSwitch>();
    }
}
