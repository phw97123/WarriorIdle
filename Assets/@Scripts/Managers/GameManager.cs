using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class GameManager
{
    // Player 
    public PlayerController Player { get { return Managers.ObjectManager?.Player; } }

    // Stage
    public event Action<StageData> OnStageUiUpdate;
    public Action<DungeonDataSO> onStartDungeon;
    public StageData StageData { get { return _stageDataSO?.GetStageData(CurrentStageIndex); } }
    private StageDataSO _stageDataSO;
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
    public void SetStageMap()
    {
        string stageName = _stageDataSO.stageDatas[CurrentStageIndex].mapName;

        GameObject go = GameObject.Find("@Map");
        if (_stageDataSO != null)
        {
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
    }

    // Kill
    private int _killCount;
    public event Action<int> OnKillCountChanged;
    public int KillCount
    {
        get { return _killCount; }
        set
        {
            _killCount = value; ;
            OnKillCountChanged?.Invoke(value);
        }
    }


    // Reward
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
        OnRewardDataLoaded.Invoke(rewards);
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

    // Equipement
    private int _rarityMaxLevel = 4;
    public Dictionary<Define.EquipmentType, List<EquipmentData>> AllEquipmentDatas { get; set; }

    public void Init()
    {
        AllEquipmentDatas = new Dictionary<Define.EquipmentType, List<EquipmentData>>();

        _stageDataSO = Managers.ResourceManager.Load<StageDataSO>("StageDataSO.asset");
        CurrentStageIndex = 0;

        CreateAllWeapon();
        CreateAllArmor();
    }


    private void CreateAllWeapon()
    {
        int prevEffect = 0;
        int rarityEffect = 1;

        List<EquipmentData> weaponDatas = new List<EquipmentData>();
        foreach (Rarity rarity in Enum.GetValues(typeof(Rarity)))
        {
            for (int rarityLevel = 1; rarityLevel <= _rarityMaxLevel; rarityLevel++)
            {
                string typeName = $"Weapon{rarity}{rarityLevel}";
                string name = GetRarityName(rarity);
                Sprite icon = Managers.ResourceManager.Load<Sprite>(typeName + ".sprite");
                int equippedEffect = 3 + prevEffect + rarityEffect;
                int upgradeStone = 150 + prevEffect + rarityEffect;


                EquipmentData data = new EquipmentData(typeName, name, icon, rarityLevel, EquipmentType.Weapon, rarity, equippedEffect, upgradeStone);
                weaponDatas.Add(data);
                prevEffect = equippedEffect;
            }
            rarityEffect += 6;
        }
        AllEquipmentDatas.Add(EquipmentType.Weapon, weaponDatas);
    }

    private void CreateAllArmor()
    {
        int prevEffect = 0;
        int rarityEffect = 1;
        List<EquipmentData> armorDatas = new List<EquipmentData>();
        foreach (Rarity rarity in Enum.GetValues(typeof(Rarity)))
        {
            for (int rarityLevel = 1; rarityLevel <= _rarityMaxLevel; rarityLevel++)
            {
                string typeName = $"Armor{rarity}{rarityLevel}";
                string name = GetRarityName(rarity);
                Sprite icon = Managers.ResourceManager.Load<Sprite>(typeName + ".sprite");
                int equippedEffect = 1 + prevEffect + rarityEffect;
                int upgradeStone = 150 + prevEffect + rarityEffect;

                EquipmentData data = new EquipmentData(typeName, name, icon, rarityLevel, EquipmentType.Armor, rarity, equippedEffect, upgradeStone);
                armorDatas.Add(data);
                prevEffect = equippedEffect;
            }
            rarityEffect += 3;
        }
        AllEquipmentDatas.Add(EquipmentType.Armor, armorDatas);
    }

    public string GetRarityName(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return "ÀÏ¹Ý";
            case Rarity.Rare:
                return "·¹¾î";
            case Rarity.Ancient:
                return "Èñ±Í";
            case Rarity.Epic:
                return "¿¡ÇÈ";
            case Rarity.Heroic:
                return "¿µ¿õ";
            case Rarity.Legendary:
                return "Àü¼³";
            case Rarity.Mythical:
                return "½ÅÈ­";
        }
        return null;
    }

    public EquipmentData GetEquipmentData(EquipmentType type, string name)
    {
        List<EquipmentData> datas = AllEquipmentDatas[type];
        foreach (EquipmentData data in datas)
        {
            if (data.typeName == name)
                return data;
        }
        return null;
    }
}
