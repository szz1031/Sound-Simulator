using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LightSwitch : InteractorBase {
	public GameObject[] Lights;
	public ReflectionProbe[] ReflectionProbes;
	public Renderer[] EmissiveObjects;
	private Color[] EmissColors;
	public bool B_LightsOn = true;
    public string MainAudioName = "LightSwitch";
	private Animation m_Animation;
	private float _timer = 0.5f;
    string m_AnimName;
	protected override void Awake () {
        base.Awake();
	    m_Animation = GetComponent<Animation> ();
        if(m_Animation)
        m_AnimName = GetFistClip().name;
		foreach (GameObject _light in Lights)
        {
            if (_light)
            {
                _light.SetActive(B_LightsOn);
            }
        }

		if (B_LightsOn) {
			if (m_Animation != null) {
				m_Animation [m_AnimName].normalizedTime = 0;
				m_Animation [m_AnimName].speed = -1;
				m_Animation.Play ();
			}
		} else {
			if (m_Animation != null) {
				m_Animation [m_AnimName].normalizedTime = 1;
				m_Animation [m_AnimName].speed = 1;
				m_Animation.Play ();
			}
		}

		foreach (ReflectionProbe _probe in ReflectionProbes) {
			if (_probe) {
				_probe.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
				_probe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting;
				Invoke ("BakeProbes", _timer);
			}
		}
	
		EmissColors = new Color[EmissiveObjects.Length];

		for(int i = 0; i < EmissiveObjects.Length; i++){
			EmissColors[i] = EmissiveObjects[i].material.GetColor("_EmissionColor");
		}

		if (!B_LightsOn) {
			DisableEmission ();
		} 
		else {
			StartCoroutine (EnableEmissionLate());
		}
	}
    
	IEnumerator EnableEmissionLate () {
		yield return null;
		for(int i = 0; i < EmissiveObjects.Length; i++){
			EmissiveObjects[i].material.SetColor ("_EmissionColor", EmissColors[i]);
			DynamicGI.SetEmissive(EmissiveObjects[i], EmissColors[i] * 0.36f);
		}
	}

	void DisableEmission(){

		for (int i = 0; i < EmissiveObjects.Length; i++) {
				EmissiveObjects[i].material.SetColor ("_EmissionColor", Color.black);
				DynamicGI.SetEmissive(EmissiveObjects[i], Color.black * 0);
		}

	}
	void EnableEmission(){
		for(int i = 0; i < EmissiveObjects.Length; i++){
				EmissiveObjects[i].material.SetColor ("_EmissionColor", EmissColors[i]);
				DynamicGI.SetEmissive(EmissiveObjects[i], EmissColors[i] * 0.36f);
		}
	}

	void BakeProbes(){
		foreach (ReflectionProbe _probe in ReflectionProbes) {
			if (_probe) {
				_probe.RenderProbe ();
			}
		}
	}
	void Light_Off(){
		foreach (GameObject _light in Lights) {
			if(_light){
				_light.SetActive (false);
			}
		}

		if (m_Animation != null) {
			m_Animation [m_AnimName].normalizedTime = 1;
			m_Animation [m_AnimName].speed = 1;
			m_Animation.Play ();
		}

		Invoke("BakeProbes", _timer);

	}
	void Light_On(){
		foreach (GameObject _light in Lights) {
			if(_light){
				_light.SetActive (true);
			}
		}

		if (m_Animation != null) {
			m_Animation [m_AnimName].normalizedTime = 0;
			m_Animation [m_AnimName].speed = -1;
			m_Animation.Play ();
		}

		Invoke("BakeProbes", _timer);
	}
    public override bool TryInteract()
    {
        base.TryInteract();
        if (GameManager.Instance.B_SearchMode)
            return false;
        Switch(!B_LightsOn);
        return true;
    }

    public void Switch(bool on)
    {
        if (B_LightsOn == on)
            return;

        B_LightsOn = on;
        if (on)
        {
            Light_On();
            EnableEmission();
        }
        else
        {
            Light_Off();
            DisableEmission();
        }
        AudioManager.PostEvent(MainAudioName + (B_LightsOn ? "_On" : "_Off"), this.gameObject);
    }

    AnimationState GetFistClip()
    {
        foreach (AnimationState state in m_Animation)
            return state;
        return null;
    }
}
