using System;
using System.Collections.Generic;

public class CurrencyManager
{
    public event Action OnChangedCurrency;
    public CurrencyDataCollection currencyDataCollection;
    public List<CurrencyData> currencyDatas = new List<CurrencyData>();

    public void Init()
    {
        currencyDataCollection = Managers.JsonManager.LoadData<CurrencyDataCollection>("CurrencyDataCollection");

        if (currencyDataCollection == null)
        {
            currencyDataCollection = new CurrencyDataCollection(); 
            CurrencyData gold = new CurrencyData(Define.CurrencyType.Gold, "1000");
            CurrencyData dia = new CurrencyData(Define.CurrencyType.Dia, "1000");
            CurrencyData upgradeStone = new CurrencyData(Define.CurrencyType.UpgradeStone, "1000");
            CurrencyData skillUpgradeStone = new CurrencyData(Define.CurrencyType.SkillUpgradeStone, "0");
            CurrencyData upgradeStoneKey = new CurrencyData(Define.CurrencyType.UpgradeStoneKey, "2");
            CurrencyData goldKey = new CurrencyData(Define.CurrencyType.GoldKey, "2");

            currencyDatas.Add(gold);
            currencyDatas.Add(dia);
            currencyDatas.Add(upgradeStone);
            currencyDatas.Add(skillUpgradeStone);
            currencyDatas.Add(upgradeStoneKey);
            currencyDatas.Add(goldKey);
            currencyDataCollection.currecntyDataList = currencyDatas;
        }
        else
        {
            currencyDatas = currencyDataCollection.currecntyDataList; 
        }

        foreach (var currencyData in currencyDatas)
        {
            OnChangedCurrency?.Invoke();
        }
    }

    public void AddCurrency(Define.CurrencyType currencyType, int value)
    {
        CurrencyData data = currencyDatas.Find(c => c.currencyType == currencyType);
        if (data != null)
        {
            data.Add(value);
            OnChangedCurrency?.Invoke();
        }
    }

    public bool SubtractCurrency(Define.CurrencyType currencyType, int value)
    {
        CurrencyData data = currencyDatas.Find(c => c.currencyType == currencyType);
        if (data != null)
        {
            bool result = data.subtract(value);
            if (result)
                OnChangedCurrency?.Invoke();

            return result;
        }
        return false;
    }

    public string GetCurrencyAmount(Define.CurrencyType currencyType)
    {
        CurrencyData data = currencyDatas.Find(c => c.currencyType == currencyType);
        return data?.amount ?? "0";
    }

    public void Destroy()
    {
        OnChangedCurrency = null;
        currencyDatas.Clear();
    }
}
