using System;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

[System.Serializable]
public class StageData
{
    private List<StageDataSO> _stageDatas;

    [SerializeField] private int killCount;
    public event Action<int> OnKillCountChanged;
    public int KillCount
    {
        get { return killCount; }
        set
        {
            killCount = value; ;
            OnKillCountChanged?.Invoke(value);
        }
    }

    [SerializeField] private int stageIndex;
    public event Action<StageDataSO> OnStageUiUpdate;
    public int StageIndex
    {
        get { return stageIndex; }
        set
        {
            stageIndex = value;
            if (value > _stageDatas.Count - 1)
                stageIndex = _stageDatas.Count - 1;

            OnStageUiUpdate?.Invoke(Managers.DataManager.stageData.GetStageData());
        }
    }

    public StageData()
    {
        killCount = 0;
        stageIndex = 0;
    }

    public void Init()
    {
        _stageDatas = Managers.ResourceManager.LoadAll<StageDataSO>();
    }

    public StageDataSO GetStageData()
    {
        return _stageDatas[stageIndex];
    }

    public int GetStageCount()
    {
        return _stageDatas.Count;
    }
}
