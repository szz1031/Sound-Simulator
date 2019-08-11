using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCharacterLapTop : InteractorBase {
    bool B_Finished = false;
    Action OnInteractOnce;
    public void AddOnIntereractOnce(Action _OnInteractOnce) {
        if (B_Finished)
            return;
        OnInteractOnce = _OnInteractOnce;
    }
    public void OnFinished()
    {
        B_Finished = true;
        OnInteractOnce = () =>
        {
            GameManager.Instance.PickupKey(9);
            UIManager.Instance.AddSubtitle("Here s Safe Code");
            UIManager.Instance.AddTips("Safe Code Acquired");
            AudioManager.Play("Charatcer_02_SafeCode", this.gameObject);
        };
    }
    public override bool TryInteract()
    {
        if (OnInteractOnce!=null)
        {
            OnInteractOnce();
            OnInteractOnce = null;
        }
        
        return base.TryInteract();
    }
}
