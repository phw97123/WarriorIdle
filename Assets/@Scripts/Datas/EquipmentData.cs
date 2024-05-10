using System;
using UnityEngine;
using static Define;

public class EquipmentData 
{
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
    public int upgradeStone; 

    public EquipmentData(string rarityName, Sprite icon, int rarityLevel,EquipmentType equipmentType, Rarity rarity, int equippedEffect, int upgradeStone)
    {
        quantity = 0;
        level = 1;
        rarityMaxLevel = 4;
        isEquipped = false;

        this.equipmentType = equipmentType;
        this.rarityName = rarityName;
        this.icon = icon;
        this.rarityLevel = rarityLevel;
        this.rarity = rarity;
        this.equippedEffect = equippedEffect; 
        this.upgradeStone = upgradeStone;
    }

    public bool IsActive()
    {
        return quantity >= 1; 
    }
}
