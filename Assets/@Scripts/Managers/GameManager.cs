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

    public void Init()
    {
        InitStageData();
        InitEquipmentData();
        InitSkillData();
        InitGrowthData();
        InitSummonsData(); 
    }

    #region Growth
    public GrowthDataCollection growthCollection;
    public void InitGrowthData()
    {
        growthCollection = Managers.DataManager.LoadData<GrowthDataCollection>("GrowthDataCollection");
        List<GrowthDataSO> baseDatas = Managers.ResourceManager.LoadAll<GrowthDataSO>();
        if (growthCollection == null)
        {
            growthCollection = new GrowthDataCollection();
            foreach (var data in baseDatas)
            {
                growthCollection.growthDataList.Add(new GrowthData(data));
            }
        }
        else
        {
            foreach (var dataSO in baseDatas)
            {
                foreach (var data in growthCollection.growthDataList)
                {
                    if (data.id == dataSO.index)
                    {
                        data.baseData = dataSO;
                        break;
                    }
                }
            }
        }
    }
    #endregion

    #region Stage
    public Action<DungeonDataSO> onStartDungeon;
    public StageData stageData;

    private void InitStageData()
    {
        stageData = Managers.DataManager.LoadData<StageData>("StageData");
        if (stageData == null)
        {
            stageData = new StageData();
        }
        stageData.Init();
    }

    public void SetStageMap()
    {
        string stageName = stageData.GetStageData().mapName;

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

    #region Equipment
    private int _rarityMaxLevel = 4;
    public Dictionary<EquipmentType, List<EquipmentData>> AllEquipmentDatas { get; set; }
    public EquipmentCollection equipmentCollection;

    public bool isEquipmentDataInit = true; 
    private void InitEquipmentData()
    {
        equipmentCollection = Managers.DataManager.LoadData<EquipmentCollection>("EquipmentCollection");
        AllEquipmentDatas = new Dictionary<EquipmentType, List<EquipmentData>>();

        if (equipmentCollection == null)
        {
            equipmentCollection = new EquipmentCollection();
            CreateAllWeapon();
            CreateAllArmor();
            isEquipmentDataInit = false; 
        }
        else
        {
            List<EquipmentData> weaponDatas = new List<EquipmentData>();
            List<EquipmentData> armorDatas = new List<EquipmentData>();
            foreach (var data in equipmentCollection.equipmentDataList)
            {
                if (data.equipmentType == EquipmentType.Weapon)
                {
                    string typeName = $"Weapon{data.rarity}{data.rarityLevel}";
                    Sprite icon = Managers.ResourceManager.Load<Sprite>(typeName + ".sprite");
                    data.icon = icon;
                    weaponDatas.Add(data);
                }
                else if (data.equipmentType == EquipmentType.Armor)
                {
                    string typeName = $"Armor{data.rarity}{data.rarityLevel}";
                    Sprite icon = Managers.ResourceManager.Load<Sprite>(typeName + ".sprite");
                    data.icon = icon;
                    armorDatas.Add(data);
                }
            }
            AllEquipmentDatas[EquipmentType.Weapon] = weaponDatas;
            AllEquipmentDatas[EquipmentType.Armor] = armorDatas;
        }
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
                equipmentCollection.equipmentDataList.Add(data);
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
                equipmentCollection.equipmentDataList.Add(data);
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

    public Action<SkillData> OnAddSkillData;
    public Dictionary<SkillType, List<SkillData>> AllSkillDatas { get; set; }

    public SkillDataCollection skillDataCollection;
    public bool isSkillDataInit = true; 
    private void InitSkillData()
    {
        skillDataCollection = Managers.DataManager.LoadData<SkillDataCollection>("SkillDataCollection");

        AllSkillDatas = new Dictionary<SkillType, List<SkillData>>();

        if (skillDataCollection == null)
        {
            isSkillDataInit = false; 
            skillDataCollection = new SkillDataCollection();
            CreateSkillData();
        }
        else
        {
            LoadExistingSkillData();
        }


    }

    private void LoadExistingSkillData()
    {
        List<SkillDataSO> datas = Managers.ResourceManager.LoadAll<SkillDataSO>();

        foreach (var dataSO in datas)
        {
            UpdateSkillDataWithBaseData(dataSO);
        }

        OrganizeSkillData();
    }

    private void UpdateSkillDataWithBaseData(SkillDataSO dataSO)
    {
        foreach (var data in skillDataCollection.SkillDataList)
        {
            if (dataSO.id == data.id)
            {
                data.baseData = dataSO;
                break;
            }
        }
    }

    private void OrganizeSkillData()
    {
        List<SkillData> activeDatas = new List<SkillData>();
        List<SkillData> passiveDatas = new List<SkillData>();

        foreach (SkillData data in skillDataCollection.SkillDataList)
        {
            if (data.baseData.skillType == SkillType.Active)
                activeDatas.Add(data);
            else if (data.baseData.skillType == SkillType.Passive)
                passiveDatas.Add(data);
        }

        AllSkillDatas[SkillType.Active] = activeDatas;
        AllSkillDatas[SkillType.Passive] = passiveDatas;
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

            skillDataCollection.SkillDataList.Add(sd);
        }
        AllSkillDatas[SkillType.Active] = activeDatas;
        AllSkillDatas[SkillType.Passive] = passiveDatas;
    }

    public void AddSkillData(SkillData data)
    {
        OnAddSkillData?.Invoke(data);
    }

    #endregion

    #region SummonsData
    public SummonsDataCollection summonsDataCollection;

    public void InitSummonsData()
    {
        summonsDataCollection = Managers.DataManager.LoadData<SummonsDataCollection>("summonsDataCollection");
        List<SummonsDataSO> dataSOs = Managers.ResourceManager.LoadAll<SummonsDataSO>();

        if (summonsDataCollection == null)
        {
            summonsDataCollection = new SummonsDataCollection();

            foreach (SummonsDataSO dataSO in dataSOs)
            {
                SummonsData data = new SummonsData(dataSO);
                summonsDataCollection.summonsDataList.Add(data);
            }
            summonsDataCollection.summonsDataList.Sort((x, y) => x.summonsDataSO.id.CompareTo(y.summonsDataSO.id));
        }
        else
        {
            foreach (SummonsDataSO dataSO in dataSOs)
            {
                foreach(var data in summonsDataCollection.summonsDataList)
                {
                    if(dataSO.id == data.id)
                    {
                        data.summonsDataSO = dataSO;
                        break; 
                    }
                }
            }
        }
    }
    #endregion


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
}
