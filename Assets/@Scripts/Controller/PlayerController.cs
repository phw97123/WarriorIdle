using System.Collections;
using Unity.VisualScripting;
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

        PlayerData.HP = PlayerData.maxHp; 

        stateMachine.ChangeState(stateMachine.IdleState);

        Type = Define.ObjectType.Player;

        return true;
    }

    private void FixedUpdate()
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

                    int damage = CalculateDamage();
                    bool critical = false; 
                    if (IsCriticalHit())
                    {
                        critical = true; 
                        damage *= (int)PlayerData.CriticalDamage; 
                    }

                    target.OnDamaged(damage, critical);

                    // Knockback
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

    private bool IsCriticalHit()
    {
        float randomNum = UnityEngine.Random.Range(0f, 1f);
        return randomNum <= PlayerData.CriticalChance; 
    }

    private int CalculateDamage()
    {
        return UnityEngine.Random.Range(PlayerData.Damage - 5, PlayerData.Damage + 10);
    }

    public override void OnDamaged(int damage, bool critical)
    {
        if (isDead)
            return;

        PlayerData.HP -= damage;
        if (PlayerData.HP <= 0)
        {
            isDead = true;
            OnDead();
        }

        base.OnDamaged(damage, critical);
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

        PlayerData.HP = PlayerData.maxHp; 

       // 무적시간
       yield return new WaitForSeconds(1.0f);
        isDead = false;
    }
}
