using System;
using System.Collections;
using UnityEngine;

public class EnemyController : CharacterBaseController
{
    [SerializeField] private Transform _damageTextPos;

    public EnemyData EnemyData { get; private set; }
    public AnimationData AnimationData { get; private set; }

    private EnemyStateMachine stateMachine;

    public event Action<int, int, int, Define.CurrencyType> OnDeath;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        EnemyData = new EnemyData();
        AnimationData = new AnimationData();
        stateMachine = new EnemyStateMachine(this);

        hp = EnemyData.HP;
        stateMachine.ChangeState(stateMachine.IdleState);

        Type = Define.ObjectType.Enemy;

        isDead = false;

        OnDeath -= Managers.GameManager.EnemyDeathRewards;
        OnDeath += Managers.GameManager.EnemyDeathRewards;

        return true;
    }

    private void FixedUpdate()
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
                    target.OnDamaged(EnemyData.Damage, false);
                }
            }
        }
    }

    public override void OnDamaged(int damage, bool critical)
    {
        base.OnDamaged(damage, critical);
        if (isDead)
            return;

        var dtc = Managers.ObjectManager.Spawn<DamageTextController>(_damageTextPos.position);

        dtc.Damage = damage;
        if (critical)
            dtc.SetColor();
    }

    public override void OnDead()
    {
        Managers.GameManager.KillCount++;
        stateMachine.ChangeState(stateMachine.DeadState);
        if (gameObject.activeInHierarchy)
            StartCoroutine(CODead());
    }

    private IEnumerator CODead()
    {
        yield return new WaitForSeconds(0.5f);

        float animationLength = Animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(animationLength);

        ItemController ic = Managers.ObjectManager.Spawn<ItemController>(transform.position);
        OnDeath?.Invoke(EnemyData.RewardExp, EnemyData.RewardGold, EnemyData.RewardEnhanceStone, ic.CurrencyType);

        Managers.ObjectManager.Despawn(this);

        hp = EnemyData.MaxHp;
        isDead = false;
    }
}
