using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    private bool _init = false;

    private void Awake()
    {
        Init();     
    }

    public virtual bool Init()
    {
        if (_init)
            return false;
        _init = true;

        return true;
    }

    public virtual void OpenUI()
    {
        gameObject.SetActive(true); 
    }

    public virtual void CloseUI()
    {
        gameObject.SetActive(false); 
    }
}
