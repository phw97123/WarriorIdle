using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonsData 
{
    public Define.SummonsType type;
    public int currentExp;
    public int maxExp;
    public int level; 
    public int maxLevel;
    public int price; 

    public SummonsData(Define.SummonsType type, int currentExp, int maxExp, int level, int maxLevel, int price)
    {
        this.type = type;
        this.currentExp = currentExp;
        this.maxExp = maxExp;
        this.level = level;
        this.maxLevel = maxLevel;
        this.price = price;
    }
}
