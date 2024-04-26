using System.Collections;
using UnityEngine;

public class PlayerController : CharacterBaseController
{
    private int _attackCount = 0;

    // Data
    public AnimationData AnimationData { get; private set; }
    public PlayerData PlayerData { get; private set; }

    // Etc
    private PlayerStateMachine stateMachine;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Debug.Log("Init"); 
        AnimationData = new AnimationData();
        PlayerData = new PlayerData();
        stateMachine = new PlayerStateMachine(this);

        hp = PlayerData.HP;
        stateMachine.ChangeState(stateMachine.IdleState);

        Type = Define.objectType.Player; 

        return true; 
    }

    private void Update()
    {
        stateMachine.Update();
    }

    // Animation Event 
    public void ComboAttack(int attackCount)
    {
        PlayerData.attackRange = _attackCount < 2 ? PlayerData.attackRange : PlayerData.lastAttackRange;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, PlayerData.attackRange);
        if (colliders != null)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider != _collider)
                {
                    EnemyController target = collider.GetComponent<EnemyController>();
                    target.OnDemeged(PlayerData.damage);
                    if (attackCount == 3)
                    {
                        Vector2 direction = (collider.transform.position - transform.position).normalized;
                        if (!target.isDead)
                            target.OnKnockback(direction * PlayerData.knockbackForce);
                    }
                }
            }
        }
    }

    public override void OnDemeged(int damage)
    {
        base.OnDemeged(damage);
        Debug.Log($"Player : {hp}");
    }

    public override void OnDead()
    {
        base.OnDead();
        stateMachine.ChangeState(stateMachine.DeadState);
        StartCoroutine(CORespawn());
    }

    private IEnumerator CORespawn()
    {
        // 부활시간
        yield return new WaitForSeconds(3.0f);

        Managers.ObjectManager.DespawnAllEnemy(); 

        stateMachine.ChangeState(stateMachine.IdleState);
        hp = PlayerData.MaxHp;

        // 무적시간
        yield return new WaitForSeconds(1.0f);
        isDead = false;
    }
}
