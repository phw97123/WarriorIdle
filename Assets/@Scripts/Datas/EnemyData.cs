using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : CharacterData
{
    private float attackRange = 1.3f; 
    private int damage = 1; 

    public float AttackRange => attackRange; 
    public int Damage => damage;

    public EnemyData()
    { 
        HP = 100;
        Speed = 8.0f; 
    }
}
