using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public PlayerController Player { get; }

    public PlayerIdleState IdleState { get; }
    public PlayerRunState RunState { get; }
    public PlayerAttackState AttackState { get; }

    public PlayerStateMachine(PlayerController player)
    {
        this.Player = player;

        IdleState = new PlayerIdleState(this);
        RunState = new PlayerRunState(this);
        AttackState = new PlayerAttackState(this);
    }
}
