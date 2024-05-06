using System;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public event Action OnChangedHp;
    public event Action<int> OnBossChangedHp;   
    private int _hp = 100;

    public int HP
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

    public float speed  = 15.0f;

    public int maxHp  = 100;
}
