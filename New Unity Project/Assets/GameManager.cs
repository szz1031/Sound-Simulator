using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SimpleSingletonMono<GameManager> {
    private void Start()
    {
        PCInputManager.Instance.AddBinding<GameManager>(enum_BindingsName.Helps, UIManager.Instance.SwitchHelpsShow);
    }
    List<int> m_KeyObtained = new List<int>();
    public bool B_CanDoorOpen(int requireKeyIndex) => m_KeyObtained.Contains(requireKeyIndex);
    public void PickupKey(int keyIndex)
    {
        m_KeyObtained.Add(keyIndex);
    }
}
