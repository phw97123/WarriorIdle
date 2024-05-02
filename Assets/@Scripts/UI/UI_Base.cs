using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class UI_Base : MonoBehaviour
{
    public virtual void OpenUI()
    {
        gameObject.SetActive(true); 
    }

    public virtual void CloseUI()
    {
        gameObject.SetActive(false); 
    }
}
