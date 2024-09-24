using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillDataCollection
{
    public List<SkillData> SkillDataList;

    public SkillDataCollection()
    {
       SkillDataList = new List<SkillData>();
    }
}


[System.Serializable]
public class SkillData
{
    public SkillDataSO baseData; 

    public int id; 
    public int level;
    public int quantity;
    public int maxQuantity;
    public bool isEquipped;
    public float effectPercent;
    public int upgradePrice;
    public int damage;

    public float cooldownTime;
    public float lastUsedTime;

    public SkillData(SkillDataSO baseData)
    {
        this.baseData = baseData;

        id = baseData.id; 
        level = 1;
        quantity = 0;
        maxQuantity = 4;
        isEquipped = false;
        effectPercent = this.baseData.effect;
        upgradePrice = this.baseData.baseUpgradePrice;

        cooldownTime = this.baseData.cool;
        lastUsedTime = -cooldownTime;

        damage = Mathf.RoundToInt(Managers.GameManager.Player.PlayerData.Damage * (1 + effectPercent / 100f));
    }

    public bool IsActive()
    {
        return quantity >= 1;
    }

    public bool CanUseSkill()
    {
        return Time.time >= lastUsedTime + cooldownTime;
    }

    public void LevelUp()
    {
        upgradePrice += level * 3;

        effectPercent += baseData.upgradePercent;

        if (baseData.skillType == Define.SkillType.Active)
        {
            damage = Mathf.RoundToInt(Managers.GameManager.Player.PlayerData.Damage * (1 + effectPercent / 100f));
        }

        level++;
    }
}
