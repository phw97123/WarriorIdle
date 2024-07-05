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
    public float effectPercent; 

    public SkillData(SkillDataSO baseData)
    {
        BaseData = baseData;

        level = 1;
        quantity = -1;
        maxQuantity = 4; 
        isEquipped = false;
        effectPercent = BaseData.effect; 
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
        effectPercent += level * 3; 
    }
}
