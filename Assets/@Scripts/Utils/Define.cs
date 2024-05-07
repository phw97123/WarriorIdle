using System.Linq;

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

    public enum ObjectType
    {
        Player,
        Enemy,
        Boss,
        Item,
        DamageText
    }

    public enum CurrencyType
    {
        Gold,
        Dia,
        EnhanceStone,
    }

    public enum StageType
    {
        Normal,
        Boss,
    }

    public enum StatusType
    {
        Damage,
        MaxHp, 
        MaxMP,
        CriticalChance,
        CriticalDamage,
    }

    public const string PLAYER_PREFAB = "Player.prefab";
    public const string ITEM_PREFAB = "Item.prefab";
    public const string DAMAGETEXT_PREFAB = "DamageText.prefab";
    public const string UIGROWTHSLOT_PREFAB = "UI_GrowthSlot.prefab";


    public const string GOLD_SPRITE = "popup elements_50";
    public const string ENHANCESTONE_SPRTE = "Cristal";

    public const float RESPAWN_TIME = 3.0f;
    public const float INVINCIBILITY_TIME = 1.0f;
}
