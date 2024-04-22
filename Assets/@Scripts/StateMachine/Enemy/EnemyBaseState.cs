using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState : IState
{
    protected EnemyStateMachine statemachine;
    protected Transform Target = null; 

    public EnemyBaseState(EnemyStateMachine statemachine)
    {
        this.statemachine = statemachine;
    }

    public virtual void Enter()
    {
    }

    public virtual void Update()
    {
        // Object Manager 생기면 변경하기 
        Target = GameObject.FindGameObjectWithTag(Enums.Tag.Player.ToString()).transform;

        if (Target != null)
        {
            if (IsAttackRange())
                statemachine.ChangeState(statemachine.AttackState);
            else
                statemachine.ChangeState(statemachine.RunState);
        }
        else
        {
            statemachine.ChangeState(statemachine.IdleState);
        }
    }

    public virtual void Exit()
    {
    }

    protected bool IsAttackRange()
    {
        float distance = Vector2.Distance(Target.position, statemachine.Enemy.transform.position);

        return statemachine.Enemy.EnemyData.AttackRange >= distance; 
    }

    protected void StartAnimation(int animationHash)
    {
        statemachine.Enemy.Animator.SetBool(animationHash, true); 
    }

    protected void StopAnimation(int animationHash)
    {
        statemachine.Enemy.Animator.SetBool(animationHash, false);
    }
}
