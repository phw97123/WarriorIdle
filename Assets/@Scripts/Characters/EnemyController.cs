using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController
{
    public EnemyData EnemyData { get; private set; }

    public void Awake()
    {
        Init(); 
        EnemyData = new EnemyData();
        hp = EnemyData.HP; 
    }

    public override void OnDemeged(int damage)
    {
        base.OnDemeged(damage);
        Debug.Log($"{hp}"); 
    }
}
