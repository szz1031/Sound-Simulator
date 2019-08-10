using UnityEngine;
using UnityEngine.Events;

public class InteractorBase : MonoBehaviour
{
    InteractItemBase[] m_Items;
    protected virtual void Awake()
    {
        m_Items = GetComponentsInChildren<InteractItemBase>();
        GetComponentsInChildren<HitCheckDynamic>().Traversal((HitCheckDynamic dynamic) => {
            dynamic.Attach(TryInteract);
        });
    }
    public virtual bool TryInteract()
    {
        m_Items.Traversal((InteractItemBase item) => { item.TryInteract(); });
        return true;
    }

}
