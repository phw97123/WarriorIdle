using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : CharacterData
{
    public float attackRange = 1.5f;
    public float lastAttackRange = 1.7f;
    public int damage = 30;
    public float knockbackForce = 150f;

    public PlayerData()
    {
        Speed = 20.0f; 
        HP = 100;
        MaxHp = 100;
    }
}
