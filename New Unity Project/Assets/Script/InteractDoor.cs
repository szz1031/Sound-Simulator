using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDoor : InteractorBase,ISingleCoroutine {
    public int I_AnimRotation=90;
    protected bool b_Playing;
    protected bool b_Opened;
    protected override void Awake()
    {
        base.Awake();
        transform.rotation = Quaternion.identity;
        b_Opened = false;
    }
    public override bool TryInteract()
    {
        if (b_Playing)
            return false;

        base.TryInteract();
        b_Playing = true;
        this.StartSingleCoroutine(0,TIEnumerators.ChangeValueTo((float value)=> {  transform.rotation=Quaternion.Euler(0,value*I_AnimRotation,0); }, b_Opened?1:0, b_Opened? 0:1,1f,()=> { b_Opened = !b_Opened;  b_Playing = false; }));
        return true;
    }
}
