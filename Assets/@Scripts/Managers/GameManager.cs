using System;
using UnityEngine;

public class GameManager
{
    // Player 
    public PlayerController Player { get { return Managers.ObjectManager?.Player; } }

    // Stage
    public StageData StageData { get { return _stageDataSO?.GetStageData(CurrentStageIndex); } }
    private StageDataSO _stageDataSO;

    public event Action<StageData> OnStageUiUpdate;

    private int _currentStageIndex;
    public int CurrentStageIndex
    {
        get { return _currentStageIndex; }
        set
        {
            _currentStageIndex = value;
            if (value > _stageDataSO.stageDatas.Length - 1)
                _currentStageIndex = _stageDataSO.stageDatas.Length - 1;

            OnStageUiUpdate?.Invoke(StageData);
        }
    }

    // Kill
    private int _killCount;
    public event Action<int> OnKillCountChanged;

    public void Init()
    {
        _stageDataSO = Managers.ResourceManager.Load<StageDataSO>("StageDataSO.asset");
        CurrentStageIndex = 0;
    }

    public int KillCount
    {
        get { return _killCount; }
        set
        {
            _killCount = value; ;
            OnKillCountChanged?.Invoke(value);
        }
    }

    public void EnemyDeathRewards(int expValue, int goldValue, int enhanceStoneValue, Define.CurrencyType type)
    {
        Player.PlayerData.Exp += expValue;

        switch (type)
        {
            case Define.CurrencyType.Gold:
                Managers.CurrencyManager.AddCurrency(Define.CurrencyType.Gold, goldValue);
                break;
            case Define.CurrencyType.UpgradeStone:
                Managers.CurrencyManager.AddCurrency(Define.CurrencyType.UpgradeStone, enhanceStoneValue);
                break;
        }
    }

    public void SetStageMap()
    {
        string stageName = _stageDataSO.stageDatas[CurrentStageIndex].mapName;

        GameObject go = GameObject.Find("@Map");
        if(_stageDataSO != null)
        {
            if (go == null)
            {
                go = new GameObject() { name = "@Map" };
                go = Managers.ResourceManager.Instantiate(stageName + ".prefab",go.transform);
                go.GetComponentInChildren<MapTileController>().MapName = stageName;
            }
            else
            {
                var map = go.GetComponentInChildren<MapTileController>();
                if (stageName == map.MapName) return;

                Managers.ResourceManager.Destroy(go.transform.GetChild(0).gameObject);
                var newMap = Managers.ResourceManager.Instantiate(stageName + ".prefab",go.transform);
                newMap.GetComponentInChildren<MapTileController>().MapName = stageName;
            }
        }
    }
}
