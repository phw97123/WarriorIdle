using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="GrowthData")]
public class GrowthDataSO : ScriptableObject
{
    public Define.StatusType type;
    public int index; 
    public string growthName;
    public int basePrice;
    public int increase;
    public float percentIncrease;
    public int priceIncrease; 
}
