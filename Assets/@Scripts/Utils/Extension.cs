using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Extension 
{
    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf; 
    }

    public static bool IsValid(this BaseController bc)
    {
        return bc == null && bc.isActiveAndEnabled;  
    }
}
