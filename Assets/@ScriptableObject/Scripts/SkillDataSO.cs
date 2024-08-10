using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[CreateAssetMenu(menuName = "SkillDataSO")]
public class SkillDataSO : ScriptableObject
{
    public GameObject prefab; 
    public Sprite icon;
    public SkillType skillType;
    public Rarity rarity;
    public string skillName;
    public int mpCost;
    public float cool;
    public float effect;
    public int upgradePercent;
    public string discription;
    public int dismantalRewardCount;
    public int baseUpgradePrice;
}
