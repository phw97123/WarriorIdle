using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class EnemyRunState : EnemyBaseState
{
    public EnemyRunState(EnemyStateMachine statemachine) : base(statemachine)
    {
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.Enemy.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Enemy.AnimationData.RunParameterHash); 
    }

    public override void Update()
    {
        base.Update();
        stateMachine.Enemy.Move(Target, stateMachine.Enemy.enemyData.characterData.speed); 
    }
}
