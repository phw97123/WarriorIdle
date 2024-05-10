using System;
using System.Collections;
using UnityEngine;

public class EnemyController : CharacterBaseController
{
    [SerializeField] private Transform _damageTextPos;

    public EnemyDataSO enemyData;
    public AnimationData AnimationData { get; private set; }

    protected EnemyStateMachine stateMachine;

    public Action<int, int, int, Define.CurrencyType> OnDeath;

    public override bool Init()
    {
        base.Init();

        if (AnimationData == null)
            AnimationData = new AnimationData();

        if (stateMachine == null)
            stateMachine = new EnemyStateMachine(this);

        enemyData.characterData.Hp = enemyData.characterData.MaxHp;
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyData.attackRange);
        if (colliders != null)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag(Define.Tag.Player.ToString()))
                {
                    PlayerController target = collider.GetComponent<PlayerController>();
                    target.OnDamaged(enemyData.damage, false);
                }
            }
        }
    }

    public override void OnDamaged(int damage, bool critical)
    {
        if (isDead)
            return;

        enemyData.characterData.Hp -= damage;
        if (enemyData.characterData.Hp <= 0)
        {
            isDead = true;
            OnDead();
        }

        var dtc = Managers.ObjectManager.Spawn<DamageTextController>(_damageTextPos.position);

        dtc.Damage = damage;
        if (critical)
            dtc.SetColor();

        base.OnDamaged(damage, critical);
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

        OnDeath?.Invoke(enemyData.rewardExp, enemyData.rewardGold, enemyData.rewardEnhanceStone, ic.CurrencyType);

        Managers.ObjectManager.Despawn(this);
    }
}
