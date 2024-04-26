using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterBaseController
{
    public EnemyData EnemyData { get; private set; }
    public AnimationData AnimationData { get; private set; }

    private EnemyStateMachine stateMachine;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        EnemyData = new EnemyData();
        AnimationData = new AnimationData();
        stateMachine = new EnemyStateMachine(this);

        hp = EnemyData.MaxHp;
        stateMachine.ChangeState(stateMachine.IdleState);

        Type = Define.objectType.Enemy;

        isDead = false;
        return true;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    // Animation Event
    public void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, EnemyData.AttackRange);
        if (colliders != null)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag(Define.Tag.Player.ToString()))
                {
                    PlayerController target = collider.GetComponent<PlayerController>();
                    target.OnDemeged(EnemyData.Damage);
                }
            }
        }
    }

    public override void OnDead()
    {
        stateMachine.ChangeState(stateMachine.DeadState);
        StartCoroutine(CODead()); 
    }

    private IEnumerator CODead()
    {
        yield return new WaitForSeconds(0.5f); 

        float animationLength = Animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        Managers.ObjectManager.Despawn(this);
    }
}
