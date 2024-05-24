using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SummonsPopup : UI_Base
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Transform _parent;

    private Action _onCloseUI;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _closeButton.onClick.AddListener(OnCloseUI);

        return true;
    }

    public void CloseButtonInteractable(bool isInteractable)
    {
        _closeButton.interactable = isInteractable;
    }

    public Transform GetParent()
    {
        return _parent;
    }

    private void OnCloseUI()
    {
        CloseUI(); 
        _onCloseUI?.Invoke(); 
    }

    public void CloseUIInjection(Action action)
    {
        _onCloseUI = action;
    }
}
