using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : CharacterData
{
    public float AttackRange { get; set; } = 1.3f;
    public int Damage { get; set; } = 10;

    // TODO : Random
    public int RewardExp { get; set; } = 30;
    public int RewardGold { get; set; } = 100;
    public int RewardEnhanceStone { get; set; } = 50; 

    public EnemyData()
    { 
        HP = 100;
        Speed = 8.0f;
        MaxHp = 100; 
    }
}
