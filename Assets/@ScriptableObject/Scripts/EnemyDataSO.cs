using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Enemy")]
public class EnemyDataSO : ScriptableObject
{
    public string enemyName; 
    public CharacterData characterData;
    public float attackRange = 1.3f;
    public int damage = 10;

    public int rewardExp = 30;
    public int rewardGold = 100;
    public int rewardUpgradeStone = 50;
    public int rewardDia; 

    public EnemyDataSO()
    {
        characterData = new CharacterData();
    }
}
