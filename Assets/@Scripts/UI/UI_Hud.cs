using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Hud : UI_Base
{
    private UI_TopBar _uiTopBar;
    private UI_BottomBar _uiBottomBar;

    private void Start()
    {
        _uiTopBar = GetComponentInChildren<UI_TopBar>();
        _uiBottomBar = GetComponentInChildren<UI_BottomBar>();
    }
}
