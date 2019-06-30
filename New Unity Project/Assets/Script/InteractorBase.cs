using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractorBase : MonoBehaviour {
    public bool PlayAudio;
    public string ClipName; 
    protected virtual void Awake()
    {

    }
    public virtual bool TryInteract()
    {
        if (PlayAudio && ClipName != "")
            AudioManager.Instance.Play(this, ClipName);
        return true;
    }

}
