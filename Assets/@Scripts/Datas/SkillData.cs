using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillData
{
    public SkillDataSO BaseData { get; private set; }

    public int level; 
    public int quantity;
    public int maxQuantity;
    public bool isEquipped;
    public int damage; 

    public SkillData(SkillDataSO baseData)
    {
        BaseData = baseData;

        level = 1;
        quantity = 0;
        maxQuantity = 4; 
        isEquipped = false;
        damage = BaseData.damage; 
    }

    public void CheckLevel()
    {
        while(quantity>=maxQuantity)
        {
            LevelUp(); 
            quantity -= maxQuantity; 
        }
    }

    public void LevelUp()
    {
        level++;
        maxQuantity += 2;
        damage += level * 3; 
    }
}
