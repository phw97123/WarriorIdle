using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using TMPro.EditorUtilities;
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
    [SerializeField] private Button _settingButton;

    [SerializeField] private Text _levelUpText; 

    private PlayerData _playerData;
    private UI_SettingPanel _settingPanel;

    private CurrencyManager _currencyManager;
    private SoundManager _soundManager; 

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _currencyManager = Managers.CurrencyManager;
        _soundManager = Managers.SoundManager; 

        _playerData = Managers.ObjectManager.Player.PlayerData;

        _icon.sprite = _playerData.Icon;
        _name.text = _playerData.Name;
        _levelUpText.gameObject.SetActive(false); 

        Managers.UIManager.TryGetUIComponent(out _settingPanel);
        _settingPanel.CloseUI(false); 

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
        _playerData.OnLevelUp -= ShowLevelUpText; 
        _playerData.OnLevelUp += ShowLevelUpText; 

        _currencyManager.OnChangedCurrency -= UpdateCurrenyUI;
        _currencyManager.OnChangedCurrency += UpdateCurrenyUI;

        _settingButton.onClick.AddListener(OnClickSettingButton); 
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

        _gold.text = _currencyManager.GetCurrencyAmount(Define.CurrencyType.Gold);
        _dia.text = _currencyManager.GetCurrencyAmount(Define.CurrencyType.Dia);
        _enhanceStone.text = _currencyManager.GetCurrencyAmount(Define.CurrencyType.UpgradeStone);
    }

    private void UpdateCurrenyUI()
    {
        _gold.text = _currencyManager.GetCurrencyAmount(Define.CurrencyType.Gold);
        _dia.text = _currencyManager.GetCurrencyAmount(Define.CurrencyType.Dia);
        _enhanceStone.text = _currencyManager.GetCurrencyAmount(Define.CurrencyType.UpgradeStone); 
    }

    private void OnClickSettingButton()
    {
        _soundManager.Play(Define.UI_BUTTON); 
        _settingPanel.OpenUI(); 
    }

    private void ShowLevelUpText()
    {
        StartCoroutine(COLevelUpText());
    }

    private IEnumerator COLevelUpText()
    {
        _levelUpText.gameObject.SetActive(true);
        float totalTime = 2f;
        float blinkInterval = 0.4f;
        float timer = 0f;
        bool isActive = true; 

        while(timer <totalTime)
        {
            yield return new WaitForSeconds(blinkInterval);
            isActive = !isActive;
            _levelUpText.gameObject.SetActive(isActive); 
            timer += blinkInterval;
        }

        _levelUpText.gameObject.SetActive(false); 
    }
}
