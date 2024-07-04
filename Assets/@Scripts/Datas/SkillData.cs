using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData
{
    public SkillDataSO BaseData { get; private set; }

    public int level;
    public int quantity;
    public bool isEquipped;
    public int damage; 
}
