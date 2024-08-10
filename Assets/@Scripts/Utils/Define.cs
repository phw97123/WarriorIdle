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
        DamageText,
        DungeonObject
    }

    public enum CurrencyType
    {
        Gold,
        Dia,
        UpgradeStone,
        SkillUpgradeStone,
        GoldKey,
        UpgradeStoneKey,
    }

    public enum StageType
    {
        Normal,
        Boss,
        Dungeon
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

    // 스킬 - 일반, 레어, 에픽, 전설 
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

    public enum RewardType
    {
        Gold,
        Dia,
        UpgradeStone,
        Exp
    }

    public enum SkillType
    {
        None,
        Passive,
        Active
    }

    public enum PopupType
    {
        Equipment, 
        Skill
    }

    // Prefab
    public const string PLAYER_PREFAB = "Player.prefab";
    public const string ITEM_PREFAB = "Item.prefab";
    public const string DAMAGETEXT_PREFAB = "DamageText.prefab";
    public const string UIGROWTHSLOT_PREFAB = "UI_GrowthSlot.prefab";
    public const string UIEQUIPMENTSLOT_PREFAB = "UI_EquipmentSlot.prefab";
    public const string UISKILLSLOT_PREFAB = "UI_SkillSlot.prefab";
    public const string UISUMMONSSLOT_PREFAB = "UI_SummonsSlot.prefab";
    public const string UIEQUIPMENTICONSLOT_PREFAB = "UI_EquipmentIconSlot.prefab";
    public const string UISHOPSLOT_PREFAB = "UI_ShopSlot.prefab";
    public const string UIDUNGEONSLOT_PREFAB = "UI_DungeonSlot.prefab";
    public const string UIREWARDSLOT_PREFAB = "UI_RewardSlot.prefab";
    public const string UIREWARDPOPUP_PREFAB = "UI_RewardPopup.prefab";
    public const string LEVELUPTEXT_PREFAB = "LevelUpText.prefab"; 

    // Sprite 
    public const string GOLD_SPRITE = "popup elements_50";
    public const string DIA_SPRITE = "popup elements_48"; 
    public const string UPGRADESTONE_SPRITE = "Cristal";
    public const string SKILLUPGRADESTONE_SPRITE = "popup elements_49";
    public const string GOLDS_SPRITE = "Golds.sprite";
    public const string UPGRADESTONES_SPRITE = "UpgradeStones.sprite";
    public const string EXP_SPRITE = "Exp.sprite";

    public const string ITEM_SPRITEATLAS = "ItemAtlas.spriteatlas";

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
    public const string STARTSCENE = "StartScene.mp3";
    public const string STAGE1 = "Stage1.wav";
    public const string STAGE2 = "Stage2.mp3";
}
