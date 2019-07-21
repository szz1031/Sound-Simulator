using UnityEngine;
using UnityEngine.Events;

public class InteractorBase : MonoBehaviour {
    protected virtual void Awake()
    {
    }
    public virtual bool TryInteract()
    {
        return true;
    }

}
