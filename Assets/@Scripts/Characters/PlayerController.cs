using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : CharacterController
{
    private int _attackCount = 0;

    // Data
    public AnimationData AnimationData { get; private set; }
    public PlayerData PlayerData { get; private set; }

    // Etc
    private PlayerStateMachine stateMachine;

    private void Awake()
    {
        Init(); 
        AnimationData = new AnimationData();
        PlayerData = new PlayerData();
        hp = PlayerData.HP;
        stateMachine = new PlayerStateMachine(this);
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
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
                        if(!target.isDead)
                        target.OnKnockback(direction * PlayerData.knockbackForce);
                    }
                }
            }
        }
    }

    public override void OnDemeged(int damage)
    {
        base.OnDemeged(damage);
        Debug.Log($"{hp}"); 
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
        stateMachine.ChangeState(stateMachine.IdleState);
        hp = PlayerData.MaxHp;

        // 무적시간
        yield return new WaitForSeconds(1.0f);
        isDead = false;

        // TODO : 플레이어가 죽으면 몬스터를 전부 없애고 처음부터 시작 즉, 몬스터 리스폰  
    }
}
