using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class DataManager 
{
    public void Init()
    {
        InitGrowthData(); 
        InitStageData();
        InitEquipmentData();
        InitSkillData();
        InitSummonsData();
    }
    #region Stage
    public StageData stageData;

    private void InitStageData()
    {
        stageData = Managers.JsonManager.LoadData<StageData>("StageData");
        if (stageData == null)
        {
            stageData = new StageData();
        }
        stageData.Init();
    }
    #endregion
    #region Growth
    public GrowthDataCollection growthCollection;
    public void InitGrowthData()
    {
        growthCollection = Managers.JsonManager.LoadData<GrowthDataCollection>("GrowthDataCollection");
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
    #region Equipment
    private int _rarityMaxLevel = 4;
    public Dictionary<EquipmentType, List<EquipmentData>> AllEquipmentDatas { get; set; }
    public EquipmentCollection equipmentCollection;

    public bool isEquipmentDataInit = true;
    private void InitEquipmentData()
    {
        equipmentCollection = Managers.JsonManager.LoadData<EquipmentCollection>("EquipmentCollection");
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
        skillDataCollection = Managers.JsonManager.LoadData<SkillDataCollection>("SkillDataCollection");

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
        summonsDataCollection = Managers.JsonManager.LoadData<SummonsDataCollection>("summonsDataCollection");
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
                foreach (var data in summonsDataCollection.summonsDataList)
                {
                    if (dataSO.id == data.id)
                    {
                        data.summonsDataSO = dataSO;
                        break;
                    }
                }
            }
        }
    }
    #endregion

    public void SaveAllData()
    {
        Managers.JsonManager.SaveData(Managers.GameManager.Player.PlayerData, "PlayerData");
        Managers.JsonManager.SaveData(stageData, "StageData");
        Managers.JsonManager.SaveData(skillDataCollection, "SkillDataCollection");
        Managers.JsonManager.SaveData(equipmentCollection, "EquipmentCollection");
        Managers.JsonManager.SaveData(growthCollection, "GrowthDataCollection");
        Managers.JsonManager.SaveData(Managers.CurrencyManager.currencyDataCollection, "CurrencyDataCollection");
        Managers.JsonManager.SaveData(summonsDataCollection, "summonsDataCollection");

        Debug.Log("데이터 저장 완료");
    }

    public string GetRarityName(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return "일반";
            case Rarity.Rare:
                return "레어";
            case Rarity.Ancient:
                return "희귀";
            case Rarity.Epic:
                return "에픽";
            case Rarity.Heroic:
                return "영웅";
            case Rarity.Legendary:
                return "전설";
            case Rarity.Mythical:
                return "신화";
        }
        return null;
    }
}
