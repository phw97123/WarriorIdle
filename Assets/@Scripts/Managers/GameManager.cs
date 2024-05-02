using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager
{
    public PlayerController Player { get { return Managers.ObjectManager?.Player; } }

    public void EnemyDeathRewards(int expValue, int goldValue, int enhanceStoneValue)
    {
        // TODO : �÷��̾ ����ġ�� ��ȭ��, ��带 ��´�
        Player.PlayerData.Exp += expValue;
        Managers.CurrencyManager.AddCurrency(Define.CurrencyType.Gold, goldValue); 
        Managers.CurrencyManager.AddCurrency(Define.CurrencyType.EnhanceStone, enhanceStoneValue);
    }
}
