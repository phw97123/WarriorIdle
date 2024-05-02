using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyData 
{
    public Define.CurrencyType currencyType;
    public string amount;

    public void Add(int value)
    {
        int currentAmount = int.Parse(amount);
        currentAmount += value; 
        amount = currentAmount.ToString();
    }

    public bool subtract(int value)
    {
        int currentAmount = int.Parse(amount);
        if (currentAmount - value < 0) return false; 
        currentAmount -= value;
        amount = currentAmount.ToString();
        return true; 
    }

    public CurrencyData(Define.CurrencyType currencyType, string amount)
    {
        this.currencyType = currencyType;
        this.amount = amount;
    }
}
