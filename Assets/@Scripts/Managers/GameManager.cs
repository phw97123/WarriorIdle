using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Define;

public class GameManager
{
    // Player 
    public PlayerController Player { get { return Managers.ObjectManager?.Player; } }

    #region Stage
    public Action<DungeonDataSO> onStartDungeon;

    public void SetStageMap()
    {
        string stageName = Managers.DataManager.stageData.GetStageData().mapName;

        GameObject go = GameObject.Find("@Map");
        if (go == null)
        {
            go = new GameObject() { name = "@Map" };
            go = Managers.ResourceManager.Instantiate(stageName + ".prefab", go.transform);
            go.GetComponentInChildren<MapTileController>().MapName = stageName;
        }
        else
        {
            var map = go.GetComponentInChildren<MapTileController>();
            if (stageName == map.MapName) return;

            Managers.ResourceManager.Destroy(go.transform.GetChild(0).gameObject);
            var newMap = Managers.ResourceManager.Instantiate(stageName + ".prefab", go.transform);
            newMap.GetComponentInChildren<MapTileController>().MapName = stageName;
        }
    }
    #endregion

    #region Reward
    public event Action<RewardData[]> OnRewardDataLoaded;
    public void Rewards(RewardData[] rewards)
    {
        foreach (var reward in rewards)
        {
            switch (reward.Type)
            {
                case RewardType.Gold:
                case RewardType.Dia:
                case RewardType.UpgradeStone:
                    RewardCurrency(reward.Type, reward.Value);
                    break;

                case RewardType.Exp:
                    RewardExp(reward.Value);
                    break;
            }
        }
        OnRewardDataLoaded?.Invoke(rewards);
    }

    private void RewardExp(int amount)
    {
        if (amount > 0)
        {
            Player.PlayerData.Exp += amount;
        }
    }

    private void RewardCurrency(RewardType type, int amount)
    {
        if (amount > 0)
        {
            switch (type)
            {
                case RewardType.Gold:
                    Managers.CurrencyManager.AddCurrency(Define.CurrencyType.Gold, amount);
                    break;
                case RewardType.Dia:
                    Managers.CurrencyManager.AddCurrency(Define.CurrencyType.Dia, amount);
                    break;
                case RewardType.UpgradeStone:
                    Managers.CurrencyManager.AddCurrency(Define.CurrencyType.UpgradeStone, amount);
                    break;
            }
        }
    }
    #endregion
}
