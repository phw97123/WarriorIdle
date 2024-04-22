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

    public void ComboAttack(int attackCount)
    {
        PlayerData.attackRange = _attackCount < 3 ? PlayerData.attackRange : PlayerData.lastAttackRange;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, PlayerData.attackRange);
        if (colliders.Length > 0)
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
                        target.OnKnockback(direction * PlayerData.knockbackForce);
                    }
                }
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //}
}
