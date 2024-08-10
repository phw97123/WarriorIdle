using System.Collections;
using UnityEngine;

public class MPRegeneration : SkillBase
{
    public override void Execute(SkillData data)
    {
        StartCoroutine(COExecute(data));
    }

    public IEnumerator COExecute(SkillData data)
    {
        while (true)
        {
            if(data.CanUseSkill())
            {
                data.lastUsedTime = Time.time;
                var player = Managers.GameManager.Player;
                int maxMp = player.PlayerData.MaxMp;
                float effectPercent = data.effectPercent / 100f;
                var amount = (int)(maxMp * effectPercent);

                player.PlayerData.MP += amount;

                if (player.PlayerData.MP > maxMp)
                {
                    player.PlayerData.MP = maxMp;
                }
            }
           
            yield return null; 
        }
    }
}
