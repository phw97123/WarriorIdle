using System.Collections;
using System.Collections.Generic;
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

    private PlayerData _playerData;

    private void Start()
    {
        _playerData = Managers.ObjectManager.Player.PlayerData;

        _icon.sprite = _playerData.Icon;
        _name.text = _playerData.Name;

        // TODO : 재화 만들면 추가 
        _gold.text = "10000"; 
        _dia.text = "10000";

        UpdateExpUI();
        UpdateHpUI();
        UpdateMpUI();
        UpdateCurrenyUI();

        Bind(); 
    }

    private void Bind()
    {
        _playerData.OnChangedExp += UpdateExpUI;
        _playerData.OnChangedHp += UpdateHpUI;
        _playerData.OnChangedMp += UpdateMpUI;

        Managers.CurrencyManager.OnChangedCurrency += UpdateCurrenyUI; 
    }

    private void UpdateExpUI()
    {
        _level.text = $"Lv.{_playerData.Level}";
        _expBar.value = (float) _playerData.Exp / _playerData.MaxExp;
    }

    private void UpdateHpUI()
    {
        _hpBar.fillAmount = (float)_playerData.HP / _playerData.MaxHp;
        _hpText.text = $"{_playerData.HP} / {_playerData.MaxHp}";
    }

    private void UpdateMpUI()
    {
        _mpBar.fillAmount = (float)_playerData.MP / _playerData.MaxMp;
        _mpText.text = $"{_playerData.MP} / {_playerData.MaxMp}";
    }

    private void UpdateCurrenyUI()
    {
        _gold.text = Managers.CurrencyManager.GetCurrencyAmount(Define.CurrencyType.Gold);
        _dia.text = Managers.CurrencyManager.GetCurrencyAmount(Define.CurrencyType.Dia); 
    }
}
