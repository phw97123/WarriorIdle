using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateMachine statemachine) : base(statemachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("IdleState"); 
        StartAnimation(stateMachine.Enemy.AnimationData.IdleParameterHash);    
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Enemy.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();
    }
}
