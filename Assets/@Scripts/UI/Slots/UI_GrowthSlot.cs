using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GrowthSlot : UI_Base
{
    [SerializeField] private Text _name;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _totalIncrease;
    [SerializeField] private Text _price;
    [SerializeField] private Button _upgradeButton;

    private SoundManager _soundManager;

    private Action<Define.StatusType> _onUpgradeButton; 

    public Define.StatusType SlotType { get; set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _soundManager = Managers.SoundManager; 
        _upgradeButton.onClick.AddListener(OnClickUpgradeButton); 
        return true;
    }

    private void OnClickUpgradeButton()
    {
        _soundManager.Play(Define.UI_BUTTON);

        _onUpgradeButton.Invoke(SlotType); 
    }

    public void UpdateUI(GrowthData data)
    {
        string gold = Managers.CurrencyManager.GetCurrencyAmount(Define.CurrencyType.Gold);

        if (data.price > int.Parse(gold))
            _upgradeButton.interactable = false;
        else
            _upgradeButton.interactable = true;

        _name.text = data.baseData.growthName;
        _levelText.text = $"Lv.{data.level}"; 
        _totalIncrease.text = data.totalIncrease == 0 ? $"{data.totalIncrease}%" :$"{data.totalIncrease}";
        _price.text = $"{data.price}";
    }

    public void OnClickUpgradeButtonInjection(Action<Define.StatusType> action)
    {
        _onUpgradeButton = action; 
    }
}
