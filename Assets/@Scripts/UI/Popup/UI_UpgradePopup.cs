using System;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static Define;

public class UI_UpgradePopup : UI_Base
{
    [SerializeField] private Image _icon;
    [SerializeField] private Text _ratingText;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _effectText;
    [SerializeField] private Text _upgradeStone;
    [SerializeField] private Image _currenyIcon;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Text _currentUpgradeStone;
    [SerializeField] private Image _currentCurrenyIcon;
    [SerializeField] private Button _closeButton;

    private Action _onClickEquipmentUpgradeButton;
    private Action _onClickSkillUpgradeButton;

    private Action _onClickEquipmentCloseButton;
    private Action _onClickSkillCloseButton;

    private PopupType _currentType;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _upgradeButton.onClick.AddListener(OnClickeUpgradeButton);
        _closeButton.onClick.AddListener(OnClickCloseButton);

        return true;
    }

    public void UpdateUI(EquipmentData data)
    {
        _currentType = PopupType.Equipment;

        Sprite currenyIcon = Managers.ResourceManager.Load<SpriteAtlas>("ItemAtlas.spriteatlas").GetSprite(UPGRADESTONE_SPRITE);

        string typeText = data.equipmentType == EquipmentType.Weapon ? "피해량" : "체력";
        _icon.sprite = data.icon;
        _ratingText.text = $"{data.rarityName}{data.rarityLevel}";
        _levelText.text = $"Lv.{data.level}";
        _effectText.text = $"장착효과  {typeText} + {data.equippedEffect + data.GetUpgradeAmount()}";
        _upgradeStone.text = $"{data.upgradePrice}";
        _currenyIcon.sprite = currenyIcon;
        _currentUpgradeStone.text = Managers.CurrencyManager.GetCurrencyAmount(CurrencyType.UpgradeStone);
        _currentCurrenyIcon.sprite = currenyIcon;

        int currentUpgradeStone = int.Parse(Managers.CurrencyManager.GetCurrencyAmount(CurrencyType.UpgradeStone));
        _upgradeButton.interactable = currentUpgradeStone >= data.upgradePrice;
    }

    public void UpdateUI(SkillData data)
    {
        _currentType = PopupType.Skill;

        Sprite currenyIcon = Managers.ResourceManager.Load<SpriteAtlas>("ItemAtlas.spriteatlas").GetSprite(SKILLUPGRADESTONE_SPRITE);

        _icon.sprite = data.baseData.icon;
        _ratingText.text = Managers.DataManager.GetRarityName(data.baseData.rarity);
        _levelText.text = $"Lv.{data.level}";
        _effectText.text = $"{data.effectPercent}% => {data.baseData.upgradePercent + data.effectPercent}%";
        _upgradeStone.text = data.upgradePrice.ToString();
        _currenyIcon.sprite = currenyIcon;
        _currentUpgradeStone.text = Managers.CurrencyManager.GetCurrencyAmount(CurrencyType.SkillUpgradeStone);
        _currentCurrenyIcon.sprite = currenyIcon;

        int currentUpgradeStone = int.Parse(Managers.CurrencyManager.GetCurrencyAmount(CurrencyType.SkillUpgradeStone));
        _upgradeButton.interactable = currentUpgradeStone >= data.upgradePrice;
    }

    private void OnClickeUpgradeButton()
    {
        Managers.SoundManager.Play(UPGRADE);

        if (_currentType == PopupType.Equipment)
            _onClickEquipmentUpgradeButton?.Invoke();
        else if (_currentType == PopupType.Skill)
            _onClickSkillUpgradeButton?.Invoke();
    }

    private void OnClickCloseButton()
    {
        CloseUI();

        if (_currentType == PopupType.Equipment)
            _onClickEquipmentCloseButton?.Invoke();
        else if (_currentType == PopupType.Skill)
            _onClickSkillCloseButton?.Invoke();
    }

    public void UpgradeButtonInjection(Action onUpgrade, PopupType type)
    {
        if (type == PopupType.Equipment)
            _onClickEquipmentUpgradeButton = onUpgrade;
        else if (type == PopupType.Skill)
            _onClickSkillUpgradeButton = onUpgrade;
    }

    public void CloseButtonInjection(Action onClose, PopupType type)
    {
        if (type == PopupType.Equipment)
            _onClickEquipmentCloseButton = onClose;
        else if (type == PopupType.Skill)
            _onClickSkillCloseButton = onClose;
    }
}
