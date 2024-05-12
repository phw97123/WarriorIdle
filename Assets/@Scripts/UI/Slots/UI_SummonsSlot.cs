using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SummonsSlot : UI_Base
{
    [SerializeField] private Text _slotName;
    [SerializeField] private Image _icon;
    [SerializeField] private Text _level;
    [SerializeField] private Text _expText;
    [SerializeField] private Image _expBar;
    [SerializeField] private Button _summonsButton10;
    [SerializeField] private Button _summonsButton30;
    [SerializeField] private Button _percentageViewButton;

    public Define.SummonsType Type { get; private set; }

    private Action<int, Define.SummonsType> _onClickSummonsButton;
    private Action<SummonsData> _onClickPercentageViewButton;

    private const int _summonsCount10 = 10;
    private const int _summonsCount30 = 30;

    private SummonsData _data;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _summonsButton10.onClick.AddListener(() => { OnSummonsButton(_summonsCount10); });
        _summonsButton30.onClick.AddListener(() => { OnSummonsButton(_summonsCount30); });
        _percentageViewButton.onClick.AddListener(() => { OnClickPercentageView(_data); });

        return true;
    }

    public void SetUI(SummonsData data)
    {
        _data = data;
        Type = data.SummonsDataSO.type;
        _slotName.text = $"{data.SummonsDataSO.slotName} ¼ÒÈ¯";
        _icon.sprite = data.SummonsDataSO.icon;
        UpdateUI(data);
    }

    public void UpdateUI(SummonsData data)
    {
        _level.text = $"Lv.{data.level}";
        _expText.text = $"{data.CurrentExp}/{data.maxExp}";
        _expBar.fillAmount = (float)data.CurrentExp / data.maxExp;

        int currentDia = int.Parse(Managers.CurrencyManager.GetCurrencyAmount(Define.CurrencyType.Dia));
        _summonsButton10.interactable = currentDia >= data.Price * 10;
        _summonsButton30.interactable = currentDia >= data.Price * 30;
    }

    private void OnSummonsButton(int count)
    {
        _onClickSummonsButton?.Invoke(count, Type);
    }

    private void OnClickPercentageView(SummonsData smmonsdata)
    {
        _onClickPercentageViewButton?.Invoke(smmonsdata);
    }

    public void SummonsButtonInjection(Action<int, Define.SummonsType> action)
    {
        _onClickSummonsButton = action;
    }

    public void PercentageViewInjection(Action<SummonsData> action)
    {
        _onClickPercentageViewButton = action;
    }
}
