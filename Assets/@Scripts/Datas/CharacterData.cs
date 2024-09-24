using System;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public event Action OnChangedHp;
    public event Action<int> OnBossChangedHp;
    public int _hp = 100;
    public int _maxHp; 
    public float speed  = 5.0f;

    public int Hp
    {
        get { return _hp; }
        set
        {
            if (value < 0)
                _hp = 0;
            else
                _hp = value;

            OnChangedHp?.Invoke();
            OnBossChangedHp?.Invoke(value); 
        }
    }


    public int MaxHp
    {
        get { return _maxHp;  }
        set
        {
            _maxHp = value;
            OnChangedHp?.Invoke();
        }
    }
}
