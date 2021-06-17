using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enum_PoolSaveType
{
    Invalid = -1,
    StaticMaxAmount,
    DynamicMaxAmount,
}
internal static class PoolParent
{
    public static Transform tf_PoolSpawn = new GameObject("PoolSpawn").transform;
    public static Transform tf_PoolRegist = new GameObject("PoolRegist").transform;
}
public class ObjectPoolManager<T,Y> where Y:MonoBehaviour {

    class ItemPoolInfo
    {
        public enum_PoolSaveType e_type;
        public Y m_spawnItem;
        public int i_poolSaveAmount;
        public Action<Y> OnItemInstantiate;
        public List<Y> l_Deactive=new List<Y>();
        public List<Y> l_Active=new List<Y>();
    }

    static Dictionary<T, ItemPoolInfo> d_ItemInfos = new Dictionary<T, ItemPoolInfo>();
    public static bool Registed(T identity)
    {
        return d_ItemInfos.ContainsKey(identity);
    }
    public static Y GetRegistedSpawnItem(T identity)
    {
        return d_ItemInfos[identity].m_spawnItem;
    }
    public static void Register(T identity, Y registerItem,  enum_PoolSaveType savetype, int poolSaveAmount, Action<Y> OnItemInstantiate)
    {
        if (d_ItemInfos.ContainsKey(identity))
        {
            Debug.LogError("Same Element Already Registed:" + identity.ToString() + "/" + registerItem.gameObject.name);
            return;
        }
        d_ItemInfos.Add(identity, new ItemPoolInfo());
        registerItem.transform.SetParent(PoolParent.tf_PoolRegist);
        OnItemInstantiate?.Invoke(registerItem);
        registerItem.SetActivate(false);
        ItemPoolInfo info = d_ItemInfos[identity];
        info.m_spawnItem = registerItem;
        info.i_poolSaveAmount = poolSaveAmount;
        info.e_type = savetype;
        info.OnItemInstantiate = OnItemInstantiate;
        Y spawnItem;
        for (int i = 0; i < info.i_poolSaveAmount; i++)
        {
            spawnItem = GameObject.Instantiate(info.m_spawnItem, PoolParent.tf_PoolSpawn).GetComponent<Y>();
            spawnItem.name = info.m_spawnItem.name + (info.l_Deactive.Count + info.l_Active.Count).ToString();
            info.OnItemInstantiate?.Invoke(spawnItem);
            info.l_Deactive.Add(spawnItem);
        }
    }
    public static Y Spawn(T identity,Transform toTrans)
    {
        if (!d_ItemInfos.ContainsKey(identity))
        {
            Debug.LogWarning("Null Identity:" + identity + "Registed");
            return null;
        }
        ItemPoolInfo info = d_ItemInfos[identity];
        Y item;
        if (info.l_Deactive.Count > 0)
        {
            item = info.l_Deactive[0];
            info.l_Deactive.RemoveAt(0);
        }
        else
        {
            item = GameObject.Instantiate(info.m_spawnItem, PoolParent.tf_PoolSpawn);
            item.name = info.m_spawnItem.name+(info.l_Deactive.Count + info.l_Active.Count).ToString();
            info.OnItemInstantiate?.Invoke(item);
        }
        info.l_Active.Add(item);
        item.transform.SetParent(toTrans==null? PoolParent.tf_PoolSpawn : toTrans);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        item.transform.localScale = Vector3.one;
        item.SetActivate(true);
        return item;
    }
    public static void Recycle(T identity,Y obj)
    {
        if (!d_ItemInfos.ContainsKey(identity))
        {
            Debug.LogWarning("Null Identity Of GameObject:"+obj.name+"/"+identity+" Registed");
            return;
        }
        ItemPoolInfo info = d_ItemInfos[identity];
        info.l_Active.Remove(obj);
        if (info.e_type == enum_PoolSaveType.DynamicMaxAmount)
        {
            info.i_poolSaveAmount++;
            obj.SetActivate(false);
            obj.transform.SetParent(PoolParent.tf_PoolSpawn);
            info.l_Deactive.Add(obj);
        }
        else if (info.e_type == enum_PoolSaveType.StaticMaxAmount)
        {
            if (info.l_Deactive.Count >= info.i_poolSaveAmount)
            {
                GameObject.Destroy(obj);
            }
            else
            {
                obj.SetActivate(false);
                obj.transform.SetParent(PoolParent.tf_PoolSpawn);
                info.l_Deactive.Add(obj);
            }
        }
    }

    public static void RecycleAll(T identity)
    {
        ItemPoolInfo info = d_ItemInfos[identity];
        List<Y> ltemp = new List<Y>();
        ltemp.AddRange(info.l_Active);
        for (int i = 0; i < ltemp.Count; i++)
        {
            Recycle(identity, ltemp[i]);
        }
    }
    public static void RecycleAllManagedItems() 
    {
        d_ItemInfos.Traversal((T temp, ItemPoolInfo info)=> {
            RecycleAll(temp);
        });
    }
}
