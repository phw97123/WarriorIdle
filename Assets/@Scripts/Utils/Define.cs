public static class Define 
{
    public enum Tag
    {
        Player,
        Enemy,
    }

    public enum CharacterState
    {
        Idle,
        Run,
        Attack,
        TakeHit,
        Death
    }

    public enum objectType
    {
        Player, 
        Enemy, 
    }

    public enum CurrencyType
    {
        Gold,
        Dia,
        EnhanceStone,
    }

    public const int GOBLIN_ID = 1;
    public const int MUSHROOM_ID = 2; 
    public const int SKELETON_ID = 3;
    public const int FLYINGEYE_ID = 4;

    public const float RESPAWN_TIME = 3.0f;
    public const float INVINCIBILITY_TIME = 1.0f; 
}
