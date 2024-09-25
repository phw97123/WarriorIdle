using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_DungeonPanel : UI_Base
{
    [SerializeField] private Transform _parent;

    private Action<Transform> _onCreateSlot;
    public event Action onOpenUI; 
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        OnCreateSlot(); 

        return true; 
    }

    public override void OpenUI()
    {
        base.OpenUI();
        onOpenUI?.Invoke(); 
    }

    private void OnCreateSlot()
    {
        _onCreateSlot?.Invoke(_parent); 
    }

    public void CreateSlotInjection(Action<Transform> action)
    {
        _onCreateSlot = action;
    }
}
