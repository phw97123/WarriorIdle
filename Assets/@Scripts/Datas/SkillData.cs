using UnityEngine;

public class SkillData
{
    public SkillDataSO BaseData { get; private set; }

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
        BaseData = baseData;

        level = 1;
        quantity = 0;
        maxQuantity = 4;
        isEquipped = false;
        effectPercent = BaseData.effect;
        upgradePrice = BaseData.baseUpgradePrice;

        cooldownTime = BaseData.cool;
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

        effectPercent += BaseData.upgradePercent;

        if (BaseData.skillType == Define.SkillType.Active)
        {
            damage = Mathf.RoundToInt(Managers.GameManager.Player.PlayerData.Damage * (1 + effectPercent / 100f));
        }

        level++;
    }
}
