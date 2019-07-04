using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractorBase : AkTriggerBase {
    protected virtual void Awake()
    {
    }
    public virtual bool TryInteract()
    {
        triggerDelegate?.Invoke(this.gameObject);
        return true;
    }

}
