using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviormentManager : MonoBehaviour {
    Light m_DirectionLight;
    private void Awake()
    {
        m_DirectionLight = transform.Find("Directional Light").GetComponent<Light>();
    }
    private void Update()
    {
        m_DirectionLight.transform.Rotate(0,6f*Time.deltaTime,0, Space.World);
    }
}
