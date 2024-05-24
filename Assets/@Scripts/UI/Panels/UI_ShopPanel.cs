using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ShopPanel : UI_Base
{
    [SerializeField] private Transform _parents;

    private Action<Transform> _onCreateSlots; 

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        OnCreateSlot();

        return true; 
    }

    private void OnCreateSlot()
    {
        _onCreateSlots.Invoke(_parents); 
    }

    public void CreateSlotsInjection(Action<Transform> action)
    {
        _onCreateSlots = action;
    }
}
