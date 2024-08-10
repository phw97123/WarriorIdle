using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPRegeneration : SkillBase
{
    public override void Execute(SkillData data)
    {
        StartCoroutine(COExecute(data));
    }

    public IEnumerator COExecute(SkillData data)
    {
        while(true)
        {
            if(data.CanUseSkill())
            {
                data.lastUsedTime = Time.time;
                var player = Managers.GameManager.Player;
                int maxHp = player.PlayerData.MaxHp;
                float effectPercent = data.effectPercent / 100f;
                var amount = (int)(maxHp * effectPercent);

                player.PlayerData.Hp += amount;

                if (player.PlayerData.Hp > maxHp)
                {
                    player.PlayerData.Hp = maxHp;
                }
            }

            yield return null;
        }
    }
}
