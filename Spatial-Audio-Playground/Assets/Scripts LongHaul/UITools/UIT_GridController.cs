using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIT_GridController
{
    public Transform transform;
    protected GameObject GridItem;
    protected Dictionary<int, Transform> ActiveItemDic = new Dictionary<int, Transform>();
    protected List<Transform> InactiveItemList = new List<Transform>();
    public UIT_GridController(Transform _transform)
    {
        transform = _transform;
        GridItem = transform.Find("GridItem").gameObject;
        GridItem.gameObject.SetActive(false);
        InactiveItemList.Add(GridItem.transform);
    }
    public virtual Transform AddItem(int identity)
    {
        Transform toTrans;
        if (InactiveItemList.Count > 0)
        {
            toTrans = InactiveItemList[0];
            InactiveItemList.Remove(toTrans);
        }
        else
        {
            toTrans = GameObject.Instantiate(GridItem.gameObject, this.transform).transform;
        }
        toTrans.name = identity.ToString();
        if (ActiveItemDic.ContainsKey(identity))
        {
            Debug.LogWarning(identity + "Already Exists In Grid Dic");
        }
        else
        {
            ActiveItemDic.Add(identity, toTrans);
        }
        return toTrans;
    }
    public virtual Transform GetItem(int identity)
    {
        return ActiveItemDic[identity];
    }
    public virtual void RemoveItem(int identity)
    {
        InactiveItemList.Add(ActiveItemDic[identity]);
        ActiveItemDic[identity].SetActivate(false);
        ActiveItemDic.Remove(identity);
    }
    public virtual void ClearGrid ()
    {
        foreach (Transform trans in ActiveItemDic.Values)
        {
            trans.SetActivate(false);
            InactiveItemList.Add(trans);
        }
        ActiveItemDic.Clear();
    }
    public bool Contains(int identity)
    {
        return ActiveItemDic.ContainsKey(identity);
    }
}
public class UIT_GridControllerDefaultMono<T> : UIT_GridControllerMono<T> where T : UIT_GridDefaultItem
{
    bool b_btnEnable;
    bool b_doubleClickConfirm;
    bool b_activeHighLight;
    Action<int> OnItemSelected;
    public int I_CurrentSelecting { get; private set; }
    public UIT_GridControllerDefaultMono(Transform _transform, Action<int> _OnItemSelected = null, bool activeHighLight = true, bool doubleClickConfirm = false) : base(_transform)
    {
        b_btnEnable = true;
        b_activeHighLight = activeHighLight;
        b_doubleClickConfirm = doubleClickConfirm;
        OnItemSelected = _OnItemSelected;
        I_CurrentSelecting = -1;
    }
    public override void ClearGrid()
    {
        base.ClearGrid();
        I_CurrentSelecting = -1;
    }
    public override void DeHighlightAll()
    {
        base.DeHighlightAll();
        I_CurrentSelecting = -1;
    }
    public override void RemoveItem(int identity)
    {
        base.RemoveItem(identity);

        if (identity == I_CurrentSelecting)
            I_CurrentSelecting = -1;
    }
    public void OnItemSelect(int identity)
    {
        if (!b_btnEnable)
            return;
        if (b_doubleClickConfirm)
        {
            if (identity == I_CurrentSelecting)
            {
                OnItemSelected?.Invoke(identity);
                return;
            }
        }
        else
        {
            if (b_activeHighLight && identity == I_CurrentSelecting)
            {
                return;
            }
            OnItemSelected?.Invoke(identity);
        }

        if (b_activeHighLight && I_CurrentSelecting != -1)
            GetItem(I_CurrentSelecting).SetHighLight(false);

        I_CurrentSelecting = identity;

        if (b_activeHighLight)
            GetItem(I_CurrentSelecting).SetHighLight(true);
    }
    public void SetBtnsEnable(bool active)
    {
        b_btnEnable = active;
    }
}
public class UIT_GridControllerMono<T>:UIT_GridController where T:UIT_GridItem
{
    Dictionary<int, T> MonoItemDic = new Dictionary<int, T>();
    public int I_Count=> MonoItemDic.Count;
    public GridLayoutGroup m_GridLayout { get; private set; }
    public UIT_GridControllerMono(Transform _transform) : base(_transform)
    {
        m_GridLayout = _transform.GetComponent<GridLayoutGroup>();
    }
    public new T AddItem(int identity) 
    {
        T item = base.AddItem(identity).GetComponent<T>();
        item.SetGridControlledItem(identity,this, null);
        MonoItemDic.Add(identity,item);
        item.SetActivate(true);
        item.transform.SetSiblingIndex(identity); 
        return item;
    }
    public void SortChildrenSibling()
    {
        List<int> keyCollections = MonoItemDic.Keys.ToList();
        keyCollections.Sort((a,b)=> {return a > b?1:-1; });
        for (int i = 0; i < keyCollections.Count; i++)
            MonoItemDic[keyCollections[i]].transform.SetAsLastSibling();
    }
    public new T GetItem(int identity)
    {
        return Contains(identity)?MonoItemDic[identity]:null;
    }
    public override void ClearGrid()
    {
        base.ClearGrid();
        MonoItemDic.Clear();
    }
    public virtual void DeHighlightAll()
    {
        foreach (T template in MonoItemDic.Values)
        {
            if(template.B_HighLight)
            template.SetHighLight(false);
        }
    }
    public override void RemoveItem(int identity)
    {
        base.RemoveItem(identity);
        MonoItemDic[identity].Reset();
        MonoItemDic.Remove(identity);
    }
    public void TraversalItem(Action<int, T> onEach)
    {
        foreach (int i in ActiveItemDic.Keys)
        {
            onEach(i, MonoItemDic[i]);
        }
    }
}
