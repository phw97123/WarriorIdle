using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Hud : UI_Base
{
    public UI_TopBar uiTopBar { get; private set; }
    public UI_BottomBar uiBottomBar { get; private set; }
    public FadeController fadeController { get; private set; }


    private void Start()
    {
        uiTopBar = GetComponentInChildren<UI_TopBar>();
        uiBottomBar = GetComponentInChildren<UI_BottomBar>();
        fadeController = GetComponent<FadeController>();
    }
}
