using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillDataSO")]
public class SkillDataSO : ScriptableObject
{
    public GameObject effctPrefab; 
    public Sprite icon; 
    public string skillName;
    public int id;
    public int damage;
    public int mpCost;
    public float cool;
}
