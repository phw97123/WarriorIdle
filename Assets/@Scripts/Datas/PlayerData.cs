using System;
using UnityEngine;
using static Define;

[System.Serializable]
public class PlayerData : CharacterData
{
    // 플레이어 정보 
    public Sprite icon; 
    public string _name = ""; 
    public string Name
    {
        get { return _name; } 
        set 
        { 
            _name = value;
            OnPlayerName?.Invoke(); 
        }
    } 
    // 플레이어 스탯
    public int _level = 1;
    public float _exp = 0;
    public float _mp = 100;

    public float CriticalDamage { get; set; } = 1.5f;
    public float CriticalChance { get; set; } = 0.2f;

    public event Action OnChangedStatus;
    public event Action OnPlayerName; 
    public event Action OnLevelUp;


    public int Level
    {
        get { return _level; }
        set
        {
            if (value < 1)
                _level = 1;
            else
                _level = value;
            OnChangedStatus.Invoke();
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
            OnChangedStatus.Invoke();
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
            OnChangedStatus.Invoke();
        }
    }

    public float MaxExp { get; set; } = 100;
    public int MaxMp { get; set; } = 100;
    public float AttackRange { get; set; } = 1.5f;
    public float LastAttackRange { get; set; } = 1.7f;
    public int Damage { get; set; } = 30;
    public float KnockbackForce { get; set; } = 50;

    public PlayerData()
    {
        MaxHp = 100;
        Hp = MaxHp;
        speed = 5.0f;
    }

    private void CheckLevelUp()
    {
        while (Exp >= MaxExp)
        {
            LevelUp();
            Exp -= MaxExp;
        }
    }

    public void SetMax()
    {
        Hp = MaxHp;
        MP = MaxMp;
    }

    private void LevelUp()
    {
        OnLevelUp.Invoke(); 
        Level++;
        MaxHp += Level * 15;
        MaxExp += Level * 30;
        MaxMp += Level * 15;
        Damage += Level * 3;
        Hp = MaxHp;
        MP = MaxMp;
    }

    public void UpgradeStatus(StatusType type, int increase, float percentIncrease)
    {
        switch (type)
        {
            case StatusType.Damage:
                Damage += increase; 
                break;
            case StatusType.MaxHp:
                MaxHp += increase;
                break;
            case StatusType.MaxMP:
                MaxMp += increase;
                break;
            case StatusType.CriticalChance:
                CriticalChance += percentIncrease; 
                break;
            case StatusType.CriticalDamage:
                CriticalDamage += percentIncrease; 
                break;
        }
    }
}
