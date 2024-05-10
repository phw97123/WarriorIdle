using System;
using System.Collections.Generic;

public class CurrencyManager 
{
    public event Action OnChangedCurrency; 
    public List<CurrencyData> currencyDatas = new List<CurrencyData>();

    public void Init()
    {
        CurrencyData gold = new CurrencyData(Define.CurrencyType.Gold, "1000");
        CurrencyData dia = new CurrencyData(Define.CurrencyType.Dia, "1000");
        CurrencyData upgradeStone = new CurrencyData(Define.CurrencyType.UpgradeStone, "1000");

        currencyDatas.Add(gold);
        currencyDatas.Add(dia);
        currencyDatas.Add(upgradeStone);

        foreach(var currencyData in currencyDatas)
        {
            OnChangedCurrency?.Invoke(); 
        }
    }

    public void AddCurrency(Define.CurrencyType currencyType, int value)
    {
        CurrencyData data = currencyDatas.Find(c=> c.currencyType == currencyType);
        if(data != null)
        {
            data.Add(value);
            OnChangedCurrency?.Invoke(); 
        }
    }

    public bool SubtractCurrency(Define.CurrencyType currencyType, int value)
    {
        CurrencyData data = currencyDatas.Find(c=>c.currencyType == currencyType);
        if(data != null)
        {
            bool result = data.subtract(value);
            if(result)
                OnChangedCurrency?.Invoke();

            return result; 
        }
        return false; 
    }

    public string GetCurrencyAmount(Define.CurrencyType currencyType)
    {
        CurrencyData data = currencyDatas.Find(c=>c.currencyType==currencyType);
        return data?.amount ?? "0"; 
    }
}
