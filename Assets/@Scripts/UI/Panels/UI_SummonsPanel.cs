using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UI_SummonsPanel : UI_Base
{
    //[SerializeField] private Transform _parent;

    private Action<Transform> _onCreatedSlots; 

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        //OnCreatedSlots(_parent);

        return true; 
    }

    private void OnCreatedSlots(Transform parent)
    {
        _onCreatedSlots?.Invoke(parent); 
    }

    public  void CreatedSlots(Action<Transform> action)
    {
        _onCreatedSlots = action;
    } 
}
