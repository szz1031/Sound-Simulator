using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractItemBase : MonoBehaviour
{
    public float F_InteractCooldown=1f;
    public bool B_CanInteract => (B_IgnoreSearchMode || !GameManager.Instance.B_SearchMode) && f_interactCheck <= 0;
    protected virtual bool B_IgnoreSearchMode => false;
    float f_interactCheck=-1;

    private void Update()
    {
        if (!B_CanInteract)
            f_interactCheck -= Time.deltaTime;
    }
    public virtual void TryInteract()
    {
        f_interactCheck = F_InteractCooldown;
    }
}
