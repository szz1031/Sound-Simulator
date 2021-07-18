using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSetting;
public class HitCheckBase : MonoBehaviour {

    public virtual enum_HitCheckType E_HitCheckType => enum_HitCheckType.Invalid;
    protected virtual void Awake()
    {
        gameObject.layer = E_HitCheckType.ToObjectLayer();
    }
}
