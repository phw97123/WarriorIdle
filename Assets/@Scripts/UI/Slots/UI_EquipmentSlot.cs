using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipmentSlot : UI_Base
{
    /*
     slot 변경 
    - 데이터를 받아와서 초기화 및 업데이트 
    - Equipment Slot이 아닌 UI_ItemSlot으로 변경 
     */

    [SerializeField] private Text _equipmentName;
    [SerializeField] private Text _upgradLevel;
    [SerializeField] private Image _icon;
    [SerializeField] private Toggle _selectToggle;
    [SerializeField] private GameObject _equippedText;

    private Action<EquipmentData> _onShowDataInfo;

    private EquipmentData _data;
    public EquipmentData Data { get { return _data; } }
    public bool isSelected = false;

    private SoundManager _soundManager; 

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _soundManager = Managers.SoundManager; 

        _selectToggle.onValueChanged.AddListener((isOn) =>
        {
            _soundManager.Play(Define.UI_BUTTON);

            if (!isOn) isSelected = false;
            else
            {
                isSelected = true;
                OnShowDataInfo();
            }
        });

        return true;
    }

    public void UpdateUI(EquipmentData data)
    {
        _equipmentName.text = $"{data.rarityName}{data.rarityLevel}";
        _upgradLevel.text = $"Lv.{data.level}";
        _icon.sprite = data.icon;
    }

    private void OnShowDataInfo()
    {
        _onShowDataInfo?.Invoke(Data);
    }

    public void OnEquippedText(bool activate)
    {
        _equippedText.SetActive(activate);
    }

    public void SetData(EquipmentData data)
    {
        _data = data;
    }

    public void SetToggleIsOn()
    {
        _selectToggle.isOn = true;
    }

    #region Injection
    public void ShowDataInfoInjection(Action<EquipmentData> action)
    {
        _onShowDataInfo = action;
    }
    #endregion
}
