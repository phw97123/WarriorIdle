using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopSlot : UI_Base
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _currencyIcon;
    [SerializeField] private Text _dataNameText;
    [SerializeField] private Text _priceText;
    [SerializeField] private Text _countText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private GameObject _CompletedPanel;

    private Action<int> _onClickBuyButton;
    private int _id;
    private int _price;
    private Define.CurrencyType _currencyType;

    private bool _isBuy;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _buyButton.onClick.AddListener(OnClickBuyButton);
        _CompletedPanel.SetActive(false);

        return true;
    }

    public override void OpenUI()
    {
        base.OpenUI();
        if (!_isBuy)
            UpdateBuyButton();
    }

    public void SetSlot(ShopDataSO data)
    {
        _icon.sprite = data.icon;
        _currencyIcon.sprite = data.currencyIcon;
        _dataNameText.text = data.dataName;
        _priceText.text = data.price.ToString();
        _countText.text = data.count.ToString(); 

        _id = data.id;
        _price = data.price; 
        _currencyType = data.currencyType;
    }

    private void OnClickBuyButton()
    {
        _onClickBuyButton.Invoke(_id);
        _CompletedPanel.SetActive(true);
        _isBuy = true;
    }

    private void UpdateBuyButton()
    {
        int current = int.Parse(Managers.CurrencyManager.GetCurrencyAmount(_currencyType));
        if (current >= _price)
            _buyButton.interactable = true;
        else
            _buyButton.interactable = false;
    }

    public void BuyItemInjection(Action<int> action)
    {
        _onClickBuyButton = action;
    }
}
