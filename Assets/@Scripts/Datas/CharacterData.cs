using System;

public class CharacterData
{
    public event Action OnChangedHp;

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
        }
    }

    public float Speed { get; set; } = 15.0f;

    public int MaxHp { get; set; } = 100;
}
