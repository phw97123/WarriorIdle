using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TopBar : UI_Base
{
    [SerializeField] private Image _icon;
    [SerializeField] private Text _name;
    [SerializeField] private Text _level;
    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _mpBar;
    [SerializeField] private Text _hpText;
    [SerializeField] private Text _mpText;
    [SerializeField] private Slider _expBar;
    [SerializeField] private Text _gold;
    [SerializeField] private Text _dia;
    [SerializeField] private Text _enhanceStone;
    [SerializeField] private Text _exp;

    private PlayerData _playerData;

    public override bool Init()
    {
        if (base.Init() == false)
            return false; 

        _playerData = Managers.ObjectManager.Player.PlayerData;

        _icon.sprite = _playerData.Icon;
        _name.text = _playerData.Name;

        UpdateUI();
        UpdateCurrenyUI();

        Bind();

        return true; 
    }

    private void Bind()
    {
        _playerData.OnChangedStatus -= UpdateUI;
        _playerData.OnChangedStatus += UpdateUI;
        _playerData.OnChangedHp -= UpdateUI;
        _playerData.OnChangedHp += UpdateUI; 
        Managers.CurrencyManager.OnChangedCurrency -= UpdateCurrenyUI;
        Managers.CurrencyManager.OnChangedCurrency += UpdateCurrenyUI; 
    }
  
    private void UpdateUI()
    {
        _level.text = $"Lv.{_playerData.Level}";
        _expBar.value = (float)_playerData.Exp / _playerData.MaxExp;

        _hpBar.fillAmount = (float)_playerData.Hp / _playerData.MaxHp;
        _hpText.text = $"{_playerData.Hp} / {_playerData.MaxHp}";

        _mpBar.fillAmount = (float)_playerData.MP / _playerData.MaxMp;
        _mpText.text = $"{_playerData.MP} / {_playerData.MaxMp}";

        _exp.text = $"{((float)_playerData.Exp / _playerData.MaxExp * 100):F2}%"; 

        _gold.text = Managers.CurrencyManager.GetCurrencyAmount(Define.CurrencyType.Gold);
        _dia.text = Managers.CurrencyManager.GetCurrencyAmount(Define.CurrencyType.Dia);
        _enhanceStone.text = Managers.CurrencyManager.GetCurrencyAmount(Define.CurrencyType.UpgradeStone);
    }

    private void UpdateCurrenyUI()
    {
        _gold.text = Managers.CurrencyManager.GetCurrencyAmount(Define.CurrencyType.Gold);
        _dia.text = Managers.CurrencyManager.GetCurrencyAmount(Define.CurrencyType.Dia);
        _enhanceStone.text = Managers.CurrencyManager.GetCurrencyAmount(Define.CurrencyType.UpgradeStone); 
    }
}
