using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine statemachine) : base(statemachine)
    {
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.Enemy.AnimationData.DeadParameterHash);
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Enemy.AnimationData.DeadParameterHash);
    }

    public override void Update()
    {
    }
}
