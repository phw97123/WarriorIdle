using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameManager
{
    // Player 
    public PlayerController Player { get { return Managers.ObjectManager?.Player; } }

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

    public void Init()
    {
        InitStageData();
        InitEquipmentData();
        InitSkillData();
    }

    #region Stage
    public event Action<StageData> OnStageUiUpdate;
    public Action<DungeonDataSO> onStartDungeon;
    public StageData StageData { get { return _stageDataSO?.GetStageData(CurrentStageIndex); } }
    private StageDataSO _stageDataSO;
    private int _currentStageIndex;

    private void InitStageData()
    {
        _stageDataSO = Managers.ResourceManager.Load<StageDataSO>("StageDataSO.asset");

        CurrentStageIndex = 0;
    }

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
    #endregion

    #region Equipment
    private int _rarityMaxLevel = 4;
    public Dictionary<EquipmentType, List<EquipmentData>> AllEquipmentDatas { get; set; }

    private void InitEquipmentData()
    {
        AllEquipmentDatas = new Dictionary<EquipmentType, List<EquipmentData>>();

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
    #endregion

    #region Skill

    public Dictionary<SkillType, List<SkillData>> AllSkillDatas { get; set; }

    private void InitSkillData()
    {
        AllSkillDatas = new Dictionary<SkillType, List<SkillData>>();
        CreateSkillData(); 
    }

    private void CreateSkillData()
    {
        List<SkillDataSO> datas = Managers.ResourceManager.LoadAll<SkillDataSO>();
        List<SkillData> activeDatas = new List<SkillData>();
        List<SkillData> passiveDatas = new List<SkillData>();   

        foreach (var data in datas)
        {
            SkillData sd = new SkillData(data);
            if (data.skillType == SkillType.Active)
                activeDatas.Add(sd);
            else
                passiveDatas.Add(sd); 
        }
        AllSkillDatas[SkillType.Active] = activeDatas;
        AllSkillDatas[SkillType.Passive] = passiveDatas;
    }


    #endregion

    public string GetRarityName(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return "�Ϲ�";
            case Rarity.Rare:
                return "����";
            case Rarity.Ancient:
                return "���";
            case Rarity.Epic:
                return "����";
            case Rarity.Heroic:
                return "����";
            case Rarity.Legendary:
                return "����";
            case Rarity.Mythical:
                return "��ȭ";
        }
        return null;
    }
}
