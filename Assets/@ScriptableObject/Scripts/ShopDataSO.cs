using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ShopDataSO")]
public class ShopDataSO : ScriptableObject
{
    public int id; 
    public Sprite icon;
    public Sprite currencyIcon; 
    public string dataName;
    public int price;
    public int count;
    public Define.CurrencyType currencyType;
    public Define.CurrencyType getCurrencyType; 
}
