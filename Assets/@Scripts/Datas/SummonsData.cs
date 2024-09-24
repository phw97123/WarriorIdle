using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class SummonsData 
{
    public Action<SummonsData> onExpChanged;
    public Action<SummonsData> onLevelChanged; 
    public SummonsDataSO SummonsDataSO { get; }
    public int CurrentExp { get; private set; }
    public int level; 
    public int maxExp;

    public int Price { get; } = 10;

    public SummonsData(SummonsDataSO SummonsDataSO)
    {
        CurrentExp = 0;
        level = 1;
        maxExp = 200; 

        this.SummonsDataSO = SummonsDataSO; 
    }

    public void AddExp (int addValue)
    {
        CurrentExp += addValue; 
        while(CurrentExp >= maxExp && level < SummonsDataSO.maxLevel)
        {
            LevelUp();
           
        }
        onExpChanged?.Invoke(this); 
    }

    private void LevelUp()
    {
        level++;
        CurrentExp -= maxExp;
        maxExp += maxExp / 5;
        onLevelChanged?.Invoke(this);
    }
}
