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
        PlayerData.AttackRange = _attackCount < 2 ? PlayerData.AttackRange : PlayerData.LastAttackRange;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, PlayerData.AttackRange);
        if (colliders != null)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    EnemyController target = collider.GetComponent<EnemyController>();
                    if (target == null) return;
                    target.OnDemeged(PlayerData.Damage);
                    if (attackCount == 3)
                    {
                        Vector2 direction = (collider.transform.position - transform.position).normalized;
                        if (!target.isDead)
                            target.OnKnockback(direction * PlayerData.KnockbackForce);
                    }
                }
            }
        }
    }

    public override void OnDemeged(int damage)
    {
        base.OnDemeged(damage);
        PlayerData.HP = hp; 
    }

    public override void OnDead()
    {
        stateMachine.ChangeState(stateMachine.DeadState);
        StartCoroutine(CORespawn());
    }

    private IEnumerator CORespawn()
    {
        // 부활시간
        yield return new WaitForSeconds(3.0f);

        Managers.ObjectManager.DespawnAllEnemy();

        stateMachine.ChangeState(stateMachine.IdleState);
        PlayerData.HP = PlayerData.MaxHp; 
        hp = PlayerData.HP;

        // 무적시간
        yield return new WaitForSeconds(1.0f);
        isDead = false;
    }
}
