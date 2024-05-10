using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_EquipmentPanel : UI_Base
{
    [SerializeField] private Toggle _weaponTab;
    [SerializeField] private Toggle _armorTab;

    [SerializeField] private Button _autoEquipButton;
    [SerializeField] private Button _allCombineButton;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _equipButton;
    [SerializeField] private Button _unequipButton;

    // slot info 
    [SerializeField] private Image _icon;
    [SerializeField] private Text _equipmentName;
    [SerializeField] private Text _upgradLevel;
    [SerializeField] private Text _equipEffectText;
    [SerializeField] private Slider _quantitySlider;
    [SerializeField] private Text _quantityText;

    [SerializeField] private Transform _slotParent;

    private Action<Transform> _onCreateSlots;
    private Action<EquipmentType> _onChangedTab;
    private Action _onClickEquipped;
    private Action _onClickUnEquipped;
    private Action<EquipmentType> _onClickAutoEquipped;
    private Action<EquipmentType> _onClickAllCombineButton;
    private Action _onClickUpgrade; 

    private EquipmentType _tabType;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _tabType = EquipmentType.Weapon;

        _weaponTab.onValueChanged.AddListener((isOn) => OnChangedTab(EquipmentType.Weapon));
        _armorTab.onValueChanged.AddListener((isOn) => OnChangedTab(EquipmentType.Armor));
        _equipButton.onClick.AddListener(OnClickEquippedButton);
        _unequipButton.onClick.AddListener(OnClickUnEquippedButton);
        _autoEquipButton.onClick.AddListener(() => OnClickAutoEquippedButton(_tabType));
        _allCombineButton.onClick.AddListener(() => OnClickCombineButton(_tabType)); 
        _upgradeButton.onClick.AddListener(OnClickUpgradeButton); 

        OnCreateSlots();

        return true;
    }

    public override void OpenUI()
    {
        base.OpenUI();
        _weaponTab.isOn = true;
    }

    private void OnCreateSlots()
    {
        _onCreateSlots?.Invoke(_slotParent);
    }

    private void OnChangedTab(EquipmentType type)
    {
        _tabType = type;
        _onChangedTab.Invoke(type);
    }

    public void UpdateSlotInfo(EquipmentData data)
    {
        string typeText = data.equipmentType == EquipmentType.Weapon ? "피해량" : "체력";
        _icon.sprite = data.icon;
        _upgradLevel.text = $"Lv.{data.level}";
        _equipmentName.text = $"{data.rarityName}{data.rarityLevel}";
        _equipEffectText.text = $"장착효과  {typeText} + {data.equippedEffect}";
        _quantitySlider.value = (float)data.quantity / 4;
        _quantityText.text = $"{data.quantity}/4";

        UpdateEquipButton(data.isEquipped);
    }

    private void OnClickEquippedButton()
    {
        _onClickEquipped.Invoke();
        UpdateEquipButton(true);
    }

    private void OnClickUnEquippedButton()
    {
        _onClickUnEquipped.Invoke();
        UpdateEquipButton(false);
    }

    private void UpdateEquipButton(bool isEquipped)
    {
        _equipButton.gameObject.SetActive(!isEquipped);
        _unequipButton.gameObject.SetActive(isEquipped);
    }

    private void OnClickAutoEquippedButton(EquipmentType type)
    {
        _onClickAutoEquipped.Invoke(type);
        UpdateEquipButton(true);
    }

    private void OnClickCombineButton(EquipmentType type)
    {
        _onClickAllCombineButton.Invoke(type);
    }

    public void IsCombineButton(bool isCombine)
    {
        _allCombineButton.interactable = isCombine; 
    }

    private void OnClickUpgradeButton()
    {
        _onClickUpgrade?.Invoke(); 
    }

    #region Injection
    public void CreateSlotsInjection(Action<Transform> action)
    {
        _onCreateSlots = action;
    }

    public void ChangedTabInjection(Action<EquipmentType> action)
    {
        _onChangedTab = action;
    }

    public void ClickEquipButtonInjection(Action action)
    {
        _onClickEquipped = action;
    }

    public void ClickUnEquipButtonInjection(Action action)
    {
        _onClickUnEquipped = action;
    }

    public void ClickAutoEquipButtonInjection(Action<EquipmentType> action)
    {
        _onClickAutoEquipped = action;
    }

    public void ClickAllCombineButton(Action<EquipmentType> action)
    {
        _onClickAllCombineButton = action;
    }

    public void ClickUpgradeButton(Action action)
    {
        _onClickUpgrade = action;
    }
    #endregion
}
