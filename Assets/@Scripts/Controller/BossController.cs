using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static Define;

public class BossController : EnemyController
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = Define.ObjectType.Boss;
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
        Sprite expSprite = Managers.ResourceManager.Load<Sprite>(EXP_SPRITE);
        Sprite diaSprite = Managers.ResourceManager.Load<SpriteAtlas>("ItemAtlas.spriteatlas").GetSprite(DIA_SPRITE);
        RewardData[] rewards = new RewardData[]
        {
           new RewardData(expSprite, enemyData.rewardExp,RewardType.Exp),
           new RewardData(diaSprite, enemyData.rewardDia,RewardType.Dia),
        };
        OnDeath?.Invoke(rewards);

        yield return new WaitForSeconds(0.5f);

        float animationLength = Animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(animationLength);
        
        Managers.ObjectManager.Despawn(this);

        isDead = false;
    }
}
