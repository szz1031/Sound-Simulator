using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIT_FpsCounter : MonoBehaviour {
    Text text;
    private void Awake()
    {
        text = GetComponent<Text>();
    }
    private void Update()
    {
        text.text = ((int)(1 / Time.deltaTime)).ToString();
    }
}
