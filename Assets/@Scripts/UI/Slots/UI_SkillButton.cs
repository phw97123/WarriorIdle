using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillButton : UI_Base
{
    [SerializeField] private Button _executeButton;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _skillCooldowntimeImage;
    [SerializeField] private Text _cooldownText;

    private SkillData _currentData = null;

    private Action<SkillData> _onClickExecute;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _executeButton.onClick.AddListener(OnClickExecuteButton);

        UpdateData(_currentData);
        return true;
    }

    public void UpdateData(SkillData data)
    {
        if (data == null)
        {
            _currentData = data;
            _icon.sprite = null;
            _icon.gameObject.SetActive(false);
            return;
        }

        _icon.gameObject.SetActive(true); 
        _currentData = data;
        _icon.sprite = _currentData.BaseData.icon;
    }


    private void OnClickExecuteButton()
    {
        if (_currentData != null)
            _onClickExecute?.Invoke(_currentData);
    }

    public SkillData GetCurrentData()
    {
        return _currentData;
    }

    public void OnClickExecuteButtonInjection(Action<SkillData> action)
    {
        _onClickExecute = action;
    }

    private void Update()
    {
        if(_currentData != null)
        {
            float remainingCooldown = Mathf.Clamp01((_currentData.cooldownTime - (Time.time - _currentData.lastUsedTime)) / _currentData.cooldownTime);
            _skillCooldowntimeImage.fillAmount = remainingCooldown;

            if (remainingCooldown > 0)
                _cooldownText.text = (remainingCooldown * _currentData.cooldownTime).ToString("F2");
            else
                _cooldownText.text = ""; 
        }
    }
}

