using System.Collections.Generic;

[System.Serializable]

public class GrowthDataCollection
{
    public List<GrowthData> growthDataList;
    public GrowthDataCollection()
    {
        growthDataList = new List<GrowthData>();
    }
}

[System.Serializable]
public class GrowthData
{
    public GrowthDataSO baseData; 
    public int id;
    public int level;
    public int price;
    public int totalIncrease;
    public float totalPercentIncrease;

    public GrowthData(GrowthDataSO baseData)
    {
        id = baseData.index; 
        level = 1;
        this.baseData = baseData;
        price = baseData.basePrice;
        totalIncrease = this.baseData.increase;
        totalPercentIncrease = this.baseData.percentIncrease;
    }
}
