using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static Define;
using UnityEngine.U2D;

public class EnemyController : CharacterBaseController
{
    [SerializeField] protected Transform _damageTextPos;

    public EnemyDataSO enemyData;
    public AnimationData AnimationData { get; private set; }

    protected EnemyStateMachine stateMachine;

    public Action<RewardData[]> OnDeath;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        if (AnimationData == null)
            AnimationData = new AnimationData();

        if (stateMachine == null)
            stateMachine = new EnemyStateMachine(this);

        stateMachine.ChangeState(stateMachine.IdleState);

        ObjectType = Define.ObjectType.Enemy;

        isDead = false;

        OnDeath -= Managers.GameManager.Rewards;
        OnDeath += Managers.GameManager.Rewards;

        return true;
    }

    private void FixedUpdate()
    {
        if (stateMachine != null)
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
        Managers.DataManager.stageData.KillCount++;
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

        RewardData[] rewards = new RewardData[]
        {
            new RewardData(null, enemyData.rewardExp,RewardType.Exp),
           new RewardData(null, enemyData.rewardGold,RewardType.Gold),
           new RewardData(null, enemyData.rewardUpgradeStone, RewardType.UpgradeStone)
        };

        OnDeath?.Invoke(rewards);

        enemyData.characterData.Hp = enemyData.characterData.MaxHp;
        isDead = false;
        Managers.ObjectManager.Despawn(this);
    }
}
