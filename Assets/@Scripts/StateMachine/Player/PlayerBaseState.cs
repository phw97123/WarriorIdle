using UnityEngine;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine; // X
    protected Transform nearestEnemy = null;

    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
    }

    public virtual void Enter()
    {
    }

    public virtual void Update()
    {
        // 패트롤 스테이트 
        nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null)
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

    // 가까운 적 찾기
    public Transform FindNearestEnemy()
    {
        Transform nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var enemyObject in GameObject.FindGameObjectsWithTag(Enums.Tag.Enemy.ToString()))
        {
            Transform enemyTransform = enemyObject.transform;
            float distance = Vector2.Distance(stateMachine.Player.transform.position, enemyTransform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemyTransform;
            }
        }

        return nearestEnemy;
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }

    protected bool IsAttackRange()
    {
        float distance = Vector2.Distance(nearestEnemy.transform.position, stateMachine.Player.transform.position);

        return stateMachine.Player.attackRange >= distance;
    }
}
