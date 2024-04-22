using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController
{
    public EnemyData EnemyData { get; private set; }
    public AnimationData AnimationData { get; private set; }

    private EnemyStateMachine stateMachine; 

    private void Awake()
    {
        Init(); 
        EnemyData = new EnemyData();
        AnimationData = new AnimationData();
        stateMachine = new EnemyStateMachine(this);
        hp = EnemyData.HP; 
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState); 
    }

    private void Update()
    {
        stateMachine.Update(); 
    }
    public override void OnDemeged(int damage)
    {
        base.OnDemeged(damage);
        Debug.Log($"{hp}"); 
    }
}
