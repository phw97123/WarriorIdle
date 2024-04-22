using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public EnemyController Enemy { get; }
    
    public EnemyIdleState IdleState { get; } 
    public EnemyRunState RunState { get; }
    public EnemyAttackState AttackState { get; }
    public EnemyDeadState DeadState { get; }

    public EnemyStateMachine(EnemyController Enemy)
    {
        this.Enemy = Enemy;

        IdleState = new EnemyIdleState(this);
        RunState = new EnemyRunState(this);
        AttackState = new EnemyAttackState(this);
        DeadState = new EnemyDeadState(this);
    }
}
