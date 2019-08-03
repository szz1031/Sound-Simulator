using UnityEngine;
using UnityEngine.Events;

public class InteractorBase : MonoBehaviour
{
    protected AudioBase[] m_Audios;
    protected virtual void Awake()
    {
        m_Audios = GetComponentsInChildren<AudioBase>();
    }
    public virtual bool TryInteract()
    {
        return true;
    }

}
