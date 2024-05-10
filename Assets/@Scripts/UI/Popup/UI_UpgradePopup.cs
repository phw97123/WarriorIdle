using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_UpgradePopup : UI_Base
{
    [SerializeField] private Image _icon;
    [SerializeField] private Text _ratingText;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _effectText;
    [SerializeField] private Text _upgradeStone;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Text _currentUpgradeStone;
    [SerializeField] private Button _closeButton; 

    private Action _onClickUpgradeButton;
    private Action _onClickCloseButton; 

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
        string typeText = data.equipmentType == EquipmentType.Weapon ? "피해량" : "체력";
        _icon.sprite = data.icon;
        _ratingText.text = $"{data.rarityName}{data.rarityLevel}";
        _levelText.text = $"Lv.{data.level}";
        _effectText.text = $"장착효과  {typeText} +{data.equippedEffect+3}";
        _upgradeStone.text = $"{data.upgradeStone}";
        _currentUpgradeStone.text = Managers.CurrencyManager.GetCurrencyAmount(Define.CurrencyType.UpgradeStone);

        int currentUpgradeStone = int.Parse(Managers.CurrencyManager.GetCurrencyAmount(Define.CurrencyType.UpgradeStone));
        _upgradeButton.interactable = currentUpgradeStone >= data.upgradeStone; 
    }

    private void OnClickeUpgradeButton()
    {
        _onClickUpgradeButton?.Invoke();
    }

    private void OnClickCloseButton()
    {
        CloseUI();
        _onClickCloseButton?.Invoke();
    }

    public void UpgradeButtonInjection(Action action)
    {
        _onClickUpgradeButton = action;
    }

    public void CloseButtonInjection(Action action)
    {
        _onClickCloseButton = action;
    }
}
