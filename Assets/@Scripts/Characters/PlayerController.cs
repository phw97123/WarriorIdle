using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : CharacterController
{
    private int _attackCount = 0;

    // Data
    public AnimationData AnimationData { get; private set; }
    public float attackRange = 1.2f;
    private float lastAttackRange = 1.35f;
    public int damage = 20;
    public float knockbackForce = 5f;

    // Etc
    private PlayerStateMachine stateMachine;

    private void Awake()
    {
        AnimationData = new AnimationData();

        Animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();

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
        attackRange = _attackCount < 3 ? attackRange : lastAttackRange;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        if (colliders.Length > 0)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider != _collider)
                {
                    EnemyController target = collider.GetComponent<EnemyController>();
                    target.OnDemeged(damage);
                    if (attackCount == 3)
                    {
                        Vector2 direction = (collider.transform.position - transform.position).normalized;
                        target.OnKnockback(direction * knockbackForce);
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
