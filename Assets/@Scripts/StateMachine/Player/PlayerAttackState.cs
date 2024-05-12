using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (nearestEnemy.isDead)
            return;
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }
}
