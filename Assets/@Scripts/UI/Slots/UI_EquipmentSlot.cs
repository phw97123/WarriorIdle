using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipmentSlot : UI_Base
{
    /*
     slot ���� 
    - �����͸� �޾ƿͼ� �ʱ�ȭ �� ������Ʈ 
    - Equipment Slot�� �ƴ� UI_ItemSlot���� ���� 
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
