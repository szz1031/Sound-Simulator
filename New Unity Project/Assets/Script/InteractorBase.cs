using UnityEngine;
using UnityEngine.Events;

public class InteractorBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        GetComponentsInChildren<HitCheckDynamic>().Traversal((HitCheckDynamic dynamic) => {
            dynamic.Attach(TryInteract);
        });
    }
    public virtual bool TryInteract()
    {
        return true;
    }

}
