using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateMachine statemachine) : base(statemachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("AttackState");

        StartAnimation(statemachine.Enemy.AnimationData.AttackParameterHash); 
    }

    public override void Exit()
    {
        StopAnimation(statemachine.Enemy.AnimationData.AttackParameterHash);
    }

    public override void Update()
    {
        base.Update();
    }
}
