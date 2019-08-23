using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameSetting
{
    public class GameConst
    {
        public const int I_PlayerInteractDistance = 1;
    }
    public enum enum_Branch
    {
        Invalid = -1,
        BranchGuitar,
    }
    public enum enum_Stage
    {
        Invalid=-1,
        Stage1=1,
        Stage2,
        Stage3,
        Stage4,
        Stage5,
    }
    public enum enum_BC_Game
    {
        Invalid=-1,
        OnStageStart,
    }
    public enum enum_HitCheckType
    {
        Invalid = -1,
        Static = 1,
        Dynamic = 2,
        Entity = 3,
    }
    public enum enum_GroundMaterialType
    {
        Invalid=-1,
        Concrete,
        Floor,
        Carpet,
        Stair,
    }
    public static class GameLayer
    {
        public static readonly int I_Static = LayerMask.NameToLayer("static");
        public static readonly int I_Dynamic = LayerMask.NameToLayer("dynamic");
        public static readonly int I_Entity = LayerMask.NameToLayer("entity");
        public static int ToObjectLayer(this enum_HitCheckType type) {
            switch (type)
            {
                default:Debug.LogError("?");return -1;
                case enum_HitCheckType.Static:
                    return I_Static;
                case enum_HitCheckType.Dynamic:
                    return I_Dynamic;
                case enum_HitCheckType.Entity:
                    return I_Entity;
            }
        }
        public static int ToCastLayer(this enum_HitCheckType type)
        {
            return 1 << ToObjectLayer(type);
        }
        public static readonly int I_SoundCastAll = 1 << I_Static;
    }
}