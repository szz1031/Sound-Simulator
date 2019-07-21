using System;
using System.Collections;
using System.Collections.Generic;
using GameSetting;
using UnityEngine;

public class HitCheckDynamic : HitCheckBase {
    public override enum_HitCheckType E_HitCheckType => enum_HitCheckType.Dynamic;
    Func<bool> TryInteract;
    public void Attach(Func<bool> _TryInteract)
    {
        TryInteract = _TryInteract;
    }
    public bool OnTryInteract()
    {
        return TryInteract();
    }
}
