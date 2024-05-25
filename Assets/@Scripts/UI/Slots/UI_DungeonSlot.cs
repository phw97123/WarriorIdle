using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_DungeonSlot : UI_Base
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _currencyIcon;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _rewardText;
    [SerializeField] private Button _startButton;
    [SerializeField] private Text _currentCurrencyText;

    private Define.CurrencyType _dungeonKey;
    private int _id;

    private Action<int> _onClickStartButton;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _startButton.onClick.AddListener(OnClickStartButton);

        return true;
    }

    public void SetSlot(DungeonDataSO data)
    {
        _id = data.id;
        _dungeonKey = data.currencyType;

        _icon.sprite = data.icon;
        _currencyIcon.sprite = data.currencyIcon;
        _nameText.text = data.dataName;
        _rewardText.text = "´øÀü È¹µæ º¸»ó: " + GetRewardText(data.getCurrencyType);
    }

    public void UpdateUI()
    {
        string keyName = Managers.CurrencyManager.GetCurrencyAmount(_dungeonKey);
        _currentCurrencyText.text = $"¿­¼è °³¼ö: {keyName}";

        _startButton.interactable = int.Parse(keyName) > 0 ? true : false;
    }

    private void OnClickStartButton()
    {
        _onClickStartButton.Invoke(_id);
        UpdateUI(); 
    }

    public void StartButtonInjection(Action<int> action)
    {
        _onClickStartButton = action;
    }

    private string GetRewardText(Define.CurrencyType type)
    {
        switch (type)
        {
            case Define.CurrencyType.Gold:
                return "°ñµå";
            case Define.CurrencyType.UpgradeStone:
                return "°­È­¼®";
        }
        return null;
    }
}
