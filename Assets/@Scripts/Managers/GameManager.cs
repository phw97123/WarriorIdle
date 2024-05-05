using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager
{
    // Player 
    public PlayerController Player { get { return Managers.ObjectManager?.Player; } }

    // Stage
    private StageDataSO _stageDataSO;
    public int CurrentStageIndex { get; set; } = 1; 
    public StageData StageData { get { return _stageDataSO?.GetStageData(CurrentStageIndex-1); } }

    // Kill
    private int _killCount;
    public event Action<int> OnKillCountChanged;

    public void Init()
    {
        _stageDataSO = Managers.ResourceManager.Load<StageDataSO>("StageDataSO.asset");
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
            case Define.CurrencyType.EnhanceStone:
                Managers.CurrencyManager.AddCurrency(Define.CurrencyType.EnhanceStone, enhanceStoneValue);
                break;
        }
    }
}
