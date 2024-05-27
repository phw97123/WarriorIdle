using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RewardPopup : UI_Base
{
    [SerializeField] private Transform _parent;
    private Action<Transform> _onCreateSlots;

    public override void OpenUI()
    {
        base.OpenUI();
        OnCreateSlots(); 
    }

    private void OnCreateSlots()
    {
        _onCreateSlots?.Invoke(_parent);
    }

    public void CreateSlotsInjection(Action<Transform> action)
    {
        _onCreateSlots = action;
    }

}
