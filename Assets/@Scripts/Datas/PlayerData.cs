using System;
using System.Runtime.InteropServices;
using UnityEditorInternal;
using UnityEngine;

public class PlayerData : CharacterData
{
    // 플레이어 정보 
    public Sprite Icon { get; set; } = null;
    public string Name { get; set; } = "phw";

    // 플레이어 스탯
    private int _level = 1;
    private float _exp = 0;
    private float _mp = 100;

    public event Action OnChangedMp;
    public event Action OnChangedExp;

    public int Level
    {
        get { return _level; }
        set
        {
            if (value < 1)
                _level = 1;
            else
                _level = value;
            OnChangedExp.Invoke();
        }
    }
    public float Exp
    {
        get { return _exp; }
        set
        {
            if (value < 0)
                _exp = 0;
            else
            {
                _exp = value;
                CheckLevelUp();
            }
            OnChangedExp.Invoke();
        }
    }
    public float MP
    {
        get { return _mp; }
        set
        {
            if (value < 0)
                _mp = 0;
            else
                _mp = value;
            OnChangedMp.Invoke();
        }
    }

    public float MaxExp { get; set; } = 100;
    public float MaxMp { get; set; } = 100;
    public float AttackRange { get; set; } = 1.5f;
    public float LastAttackRange { get; set; } = 1.7f;
    public int Damage { get; set; } = 30;
    public float KnockbackForce { get; set; } = 150;

    public PlayerData()
    {
        Speed = 20.0f;
        HP = 100;
        MaxHp = 100;
    }

    private void CheckLevelUp()
    {
        while (Exp >= MaxExp)
        {
            Level++;
            Exp -= MaxExp;
        }
    }
}
