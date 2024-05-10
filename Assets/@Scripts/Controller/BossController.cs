using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Type = Define.ObjectType.Boss;
        enemyData.characterData.Hp = enemyData.characterData.MaxHp;

        return true;
    }

    public override void OnDead()
    {
        stateMachine.ChangeState(stateMachine.DeadState);
        if (gameObject.activeInHierarchy)
            StartCoroutine(CODead());
    }

    private IEnumerator CODead()
    {
        yield return new WaitForSeconds(0.5f);

        float animationLength = Animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(animationLength);
        OnDeath?.Invoke(enemyData.rewardExp, enemyData.rewardGold, enemyData.rewardEnhanceStone, Define.CurrencyType.Gold);

        Managers.ObjectManager.Despawn(this);

        isDead = false;
    }
}
