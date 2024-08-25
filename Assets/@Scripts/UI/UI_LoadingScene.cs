using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadingScene : UI_Base
{
    [SerializeField] private Slider _loadingBar;

    public void UpdateLoadingBar(int a, int b)
    {
        _loadingBar.value = (float)a / b;
    }
}
