using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController
{
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>(); 
    }

    public override void OnDemeged(int damage)
    {
        base.OnDemeged(damage);
        Debug.Log($"-{damage}, {name}: {hp}/100"); 
    }
}
