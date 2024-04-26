using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState : IState
{
    protected EnemyStateMachine stateMachine;
    protected Transform Target = null; 

    public EnemyBaseState(EnemyStateMachine statemachine)
    {
        this.stateMachine = statemachine;
    }

    public virtual void Enter()
    {
    }

    public virtual void Update()
    {
        // Object Managers 생기면 변경하기 
        Target = null; 
        Target = GameObject.FindGameObjectWithTag(Define.Tag.Player.ToString()).transform;

        if (Target != null)
        {
            if (IsAttackRange())
                stateMachine.ChangeState(stateMachine.AttackState);
            else
                stateMachine.ChangeState(stateMachine.RunState);
        }
        else
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public virtual void Exit()
    {
    }

    protected bool IsAttackRange()
    {
        float distance = Vector2.Distance(Target.position, stateMachine.Enemy.transform.position);

        return stateMachine.Enemy.EnemyData.AttackRange >= distance; 
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Enemy.Animator.SetBool(animationHash, true); 
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Enemy.Animator.SetBool(animationHash, false);
    }
}
