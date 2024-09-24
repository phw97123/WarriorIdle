using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[System.Serializable]
public class EquipmentCollection
{
    public List<EquipmentData> equipmentDataList; 

    public EquipmentCollection() 
    {
        equipmentDataList = new List<EquipmentData>();
    }
}

[System.Serializable]
public class EquipmentData 
{
    public string typeName; 
    public string rarityName;
    public Sprite icon;
    public int quantity;
    public int level;
    public int rarityLevel;
    public int rarityMaxLevel; 
    public EquipmentType equipmentType;
    public Rarity rarity;
    public int equippedEffect;
    public bool isEquipped;
    public int upgradePrice;

    public EquipmentData(string typeName, string rarityName, Sprite icon, int rarityLevel,EquipmentType equipmentType, Rarity rarity, int equippedEffect, int upgradePrice)
    {
        quantity = 0;
        level = 1;
        rarityMaxLevel = 4;
        isEquipped = false;

        this.typeName = typeName; 
        this.equipmentType = equipmentType;
        this.rarityName = rarityName;
        this.icon = icon;
        this.rarityLevel = rarityLevel;
        this.rarity = rarity;
        this.equippedEffect = equippedEffect; 
        this.upgradePrice = upgradePrice;
    }

    public bool IsActive()
    {
        return quantity >= 1; 
    }

    public int GetUpgradeAmount()
    {
        if (equipmentType == EquipmentType.Armor)
            return 1;
        else
            return 3; 
    }
}
