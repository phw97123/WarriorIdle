using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_GrowthPanel : UI_Base
{
    private Action<Transform> _onCreateSlots;
    private Action _onSetSlots;

    [SerializeField] private Transform _slotParent;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        OnCreateSlots(); 
        return true;
    }

    public override void OpenUI()
    {
        base.OpenUI();
        OnSetSlots();
    }

    private void OnCreateSlots()
    {
        _onCreateSlots?.Invoke(_slotParent);
    }

    private void OnSetSlots()
    {
        _onSetSlots?.Invoke();
    }

    #region Injection
    public void CreateSlotsInjection(Action<Transform> action)
    {
        _onCreateSlots = action;
    }

    public void SetSlotsInjection(Action action)
    {
        _onSetSlots = action;
    }
    #endregion
}
