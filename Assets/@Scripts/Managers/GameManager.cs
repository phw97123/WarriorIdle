using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager
{
    public PlayerController Player { get { return Managers.ObjectManager?.Player; } }

    public void EnemyDeathRewards(int expValue, int goldValue, int enhanceStoneValue, Define.CurrencyType type)
    {
        // TODO : 플레이어가 경험치와 강화석, 골드를 얻는다
        Player.PlayerData.Exp += expValue;

        switch (type)
        {
            case Define.CurrencyType.Gold:
                Managers.CurrencyManager.AddCurrency(Define.CurrencyType.Gold, goldValue);
                break;
            case Define.CurrencyType.EnhanceStone:
                Managers.CurrencyManager.AddCurrency(Define.CurrencyType.EnhanceStone, enhanceStoneValue);
                break;
        }
    }
}
