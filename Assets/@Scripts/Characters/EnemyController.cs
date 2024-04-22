using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController
{
    public EnemyData EnemyData { get; private set; }
    public AnimationData AnimationData { get; private set; }

    private EnemyStateMachine stateMachine; 

    private void Awake()
    {
        Init(); 
        EnemyData = new EnemyData();
        AnimationData = new AnimationData();
        stateMachine = new EnemyStateMachine(this);
        hp = EnemyData.HP; 
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
    public void Attack ()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, EnemyData.AttackRange); 
        if(colliders != null)
        {
            foreach(Collider2D collider in colliders)
            {
                if (collider.CompareTag(Enums.Tag.Player.ToString()))
                {
                    PlayerController target = collider.GetComponent<PlayerController>();
                    target.OnDemeged(EnemyData.Damage); 
                }
            }
        }
    }
}
