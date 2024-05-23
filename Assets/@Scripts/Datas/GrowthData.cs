using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GrowthData
{
    public GrowthDataSO BaseData { get; }
    public int level;
    public int price;
    public int totalIncrease;
    public float totalPercentIncrease; 

    public GrowthData(GrowthDataSO baseData)
    {
        level = 1;
        BaseData = baseData;
        price = baseData.basePrice;
        totalIncrease = BaseData.increase; 
        totalPercentIncrease = BaseData.percentIncrease;
    }
}
