using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine;

public class UI_Hud : UI_Base
{
    public UI_TopBar UI_TopBar { get; private set; }
    public UI_BottomBar UI_BottomBar { get; private set; }
    public FadeController FadeController { get; private set; }
    public UI_StageInfo UI_StageInfo { get; private set; }
    public UI_BossStageInfo UI_BossStageInfo { get; private set; }
    public UI_DungeonStageInfo UI_DungeonStageInfo { get; private set; }

    public override bool Init()
    {
        if(base.Init() == false) 
            return false;

        UI_TopBar = GetComponentInChildren<UI_TopBar>();
        UI_BottomBar = GetComponentInChildren<UI_BottomBar>();
        UI_StageInfo = GetComponentInChildren<UI_StageInfo>();
        UI_BossStageInfo = GetComponentInChildren<UI_BossStageInfo>();
        UI_DungeonStageInfo = GetComponentInChildren<UI_DungeonStageInfo>();
        FadeController = GetComponent<FadeController>();

        return true;
    }

    public void ActivateStageInfo(Define.StageType type)
    {
        if (UI_BossStageInfo == null || UI_StageInfo == null || UI_DungeonStageInfo == null) return; 

        UI_StageInfo.gameObject.SetActive(type == Define.StageType.Normal);
        UI_BossStageInfo.gameObject.SetActive(type == Define.StageType.Boss);
        UI_DungeonStageInfo.gameObject.SetActive(type == Define.StageType.Dungeon); 
    }
}
