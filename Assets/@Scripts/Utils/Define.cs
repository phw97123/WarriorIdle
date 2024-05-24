using System.ComponentModel;
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
        UpgradeStone,
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

    public enum EquipmentType
    {
        Weapon,
        Armor
    }

    public enum SummonsType
    {
        Weapon,
        Armor,
        Skill
    }

    public enum Rarity
    {
        Common, 
        Rare,
        Epic,
        Ancient, 
        Heroic,
        Legendary,
        Mythical,
    }

    public enum AudioType
    {
        Bgm,
        Effect
    }

    // Prefab
    public const string PLAYER_PREFAB = "Player.prefab";
    public const string ITEM_PREFAB = "Item.prefab";
    public const string DAMAGETEXT_PREFAB = "DamageText.prefab";
    public const string UIGROWTHSLOT_PREFAB = "UI_GrowthSlot.prefab";
    public const string UIEQUIPMENTSLOT_PREFAB = "UI_EquipmentSlot.prefab";
    public const string UISUMMONSSLOT_PREFAB = "UI_SummonsSlot.prefab";
    public const string UIEQUIPMENTICONSLOT_PREFAB = "UI_EquipmentIconSlot.prefab"; 

    // Sprite 
    public const string GOLD_SPRITE = "popup elements_50";
    public const string ENHANCESTONE_SPRTE = "Cristal";

    // Time
    public const float RESPAWN_TIME = 3.0f;
    public const float INVINCIBILITY_TIME = 1.0f;

    // Sound - Effect
    public const string SWORD1 = "Sword1.mp3"; 
    public const string SWORD2 = "Sword2.mp3"; 
    public const string SWORD3 = "Sword3.mp3"; 
    public const string UI_BUTTON = "UIButton.mp3"; 
    public const string UPGRADE = "Upgrade.mp3";

    public const string AUDIOMIXER = "@AudioMixer.mixer"; 

    // Sound - Bgm 
    public const string STARTSCENE= "StartScene.mp3";
    public const string STAGE1 = "Stage1.wav"; 
    public const string STAGE2 = "Stage2.mp3";
    public const string DUNGEN_UPGRADESTONE = "Dungeon_UpgradeStone.mp3";
    public const string DUNGEN_GOLD = "Dungeon_Gold.mp3";
}
