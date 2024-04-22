using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.Player.AnimationData.DeadParameterHash); 
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationData.DeadParameterHash); 
    }

    public override void Update() 
    {
    }
}
