using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardData
{
    public Sprite Icon { get; private set; }
    public int Value { get; private set; }
    public Define.RewardType Type { get; private set; }
 
    public RewardData(Sprite icon, int value, Define.RewardType type)
    {
        Icon = icon;
        Value = value;
        Type = type;
    }
}
