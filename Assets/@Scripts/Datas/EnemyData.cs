using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : CharacterData
{
    private float attackRange = 1.2f;
    private int damage = 10; 

    public float AttackRange => attackRange; 
    public int Damage => damage;

    public EnemyData()
    { 
        HP = 100;
        Speed = 5.0f; 
    }
}
