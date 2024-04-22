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
        Debug.Log("RunState");
        StartAnimation(statemachine.Enemy.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        StopAnimation(statemachine.Enemy.AnimationData.RunParameterHash); 
    }

    public override void Update()
    {
        base.Update();
        statemachine.Enemy.Move(Target, statemachine.Enemy.EnemyData.Speed); 
    }
}
