using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DungeonDataSO")]
public class DungeonDataSO : ScriptableObject
{
    public int id; 
    public string dataName;
    public Sprite icon;
    public Sprite currencyIcon; 
    public Define.CurrencyType getCurrencyType;
    public Define.CurrencyType currencyType;
}
