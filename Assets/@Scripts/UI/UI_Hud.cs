using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Hud : UI_Base
{
    public UI_TopBar UI_TopBar { get; private set; }
    public UI_BottomBar UI_BottomBar { get; private set; }
    public FadeController FadeController { get; private set; }
    public UI_StageInfo UI_StageInfo { get; private set; }
    public UI_BossStageInfo UI_BossStageInfo { get; private set; }

    public override bool Init()
    {
        if(base.Init() == false) 
            return false;

        UI_TopBar = GetComponentInChildren<UI_TopBar>();
        //if (UI_TopBar != null)
        //    UI_TopBar.Init(); 

        UI_BottomBar = GetComponentInChildren<UI_BottomBar>();
        UI_StageInfo = GetComponentInChildren<UI_StageInfo>();
        //if (UI_StageInfo != null)
        //    UI_StageInfo.Init();

        UI_BossStageInfo = GetComponentInChildren<UI_BossStageInfo>();
        FadeController = GetComponent<FadeController>();

        return true;
    }

    public void ActivateStageInfo(Define.StageType type)
    {
        if (UI_BossStageInfo == null || UI_StageInfo == null) return; 

        UI_StageInfo.gameObject.SetActive(type == Define.StageType.Normal);
        UI_BossStageInfo.gameObject.SetActive(type == Define.StageType.Boss); 
    }
}
