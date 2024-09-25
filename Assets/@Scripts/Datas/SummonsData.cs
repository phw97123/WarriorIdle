using System;
using System.Collections.Generic;

[System.Serializable]
public class SummonsDataCollection
{
    public List<SummonsData> summonsDataList;
    public SummonsDataCollection()
    {
        summonsDataList = new List<SummonsData>();
    }
}

[System.Serializable]
public class SummonsData
{
    public Action<SummonsData> onExpChanged;
    public Action<SummonsData> onLevelChanged;
    public SummonsDataSO summonsDataSO;

    public int id; 
    public int CurrentExp { get; private set; }
    public int level;
    public int maxExp;

    public int Price { get; } = 10;

    public SummonsData(SummonsDataSO SummonsDataSO)
    {
        id = SummonsDataSO.id;  
        CurrentExp = 0;
        level = 1;
        maxExp = 200;

        this.summonsDataSO = SummonsDataSO;
    }

    public void AddExp(int addValue)
    {
        CurrentExp += addValue;
        while (CurrentExp >= maxExp && level < summonsDataSO.maxLevel)
        {
            LevelUp();

        }
        onExpChanged?.Invoke(this);
    }

    private void LevelUp()
    {
        if (summonsDataSO.type == Define.SummonsType.Skill) return; 
       
        level++;
        CurrentExp -= maxExp;
        maxExp += maxExp / 5;
        onLevelChanged?.Invoke(this);
    }
}
