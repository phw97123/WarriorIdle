using UnityEngine;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected Transform nearestEnemy = null;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
    }

    public virtual void Update()
    {
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

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }

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

    protected bool IsAttackRange()
    {
        float distance = Vector2.Distance(nearestEnemy.transform.position, stateMachine.Player.transform.position);

        return stateMachine.Player.PlayerData.attackRange >= distance;
    }
}
