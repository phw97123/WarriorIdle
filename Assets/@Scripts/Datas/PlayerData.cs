using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : CharacterData
{
    public float attackRange = 1.2f;
    public float lastAttackRange = 1.35f;
    public int damage = 20;
    public float knockbackForce = 5f;

    public PlayerData()
    {
        Speed = 15.0f; 
        HP = 100;
    }
}
