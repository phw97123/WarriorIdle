using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class EquipmentController : BaseController
{
    private Dictionary<EquipmentType, List<EquipmentData>> _allEquipmentDatas { get { return Managers.GameManager?.AllEquipmentDatas; } }
    private UI_EquipmentPanel _equipmentPanel;
    private UI_UpgradePopup _upgradePopup;
    private List<UI_EquipmentSlot> _slots;

    public Action OnCreateData; 

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _slots = new List<UI_EquipmentSlot>(24);

        AddPlayerEquipmentData(_allEquipmentDatas[EquipmentType.Weapon][0]);
        AddPlayerEquipmentData(_allEquipmentDatas[EquipmentType.Armor][0]);

        Managers.UIManager.TryGetUIComponent(out _equipmentPanel);
        Managers.UIManager.TryGetUIComponent(out _upgradePopup);
        _upgradePopup.gameObject.SetActive(false);

        _equipmentPanel.CreateSlotsInjection(CreateSlots);
        _equipmentPanel.ChangedTabInjection(ChangedEquipmentData);
        _equipmentPanel.ClickEquipButtonInjection(OnEquip);
        _equipmentPanel.ClickUnEquipButtonInjection(OnUnEquip);
        _equipmentPanel.ClickAutoEquipButtonInjection(OnAutoEquip);
        _equipmentPanel.ClickAllCombineButton(AllCombine);
        _equipmentPanel.ClickUpgradeButton(ShowUpgradePopup);

        _upgradePopup.UpgradeButtonInjection(UpgradeEquipment,PopupType.Equipment);
        _upgradePopup.CloseButtonInjection(UpgradePopupCloseUI, PopupType.Equipment);

        return true;
    }

    private void CreateSlots(Transform parent)
    {
        List<EquipmentData> sortedDatas = _allEquipmentDatas[EquipmentType.Weapon];

        foreach (var data in sortedDatas)
        {
            GameObject go = Managers.ResourceManager.Instantiate(UIEQUIPMENTSLOT_PREFAB, parent);
            var slot = go.GetOrAddComponent<UI_EquipmentSlot>();
            slot.transform.SetParent(parent, false);
            _slots.Add(slot);

            Toggle toggle = slot.GetComponentInChildren<Toggle>();
            toggle.group = parent.GetComponent<ToggleGroup>();

            slot.ShowDataInfoInjection(_equipmentPanel.UpdateSlotInfo);
            slot.gameObject.SetActive(data.IsActive());
        }
    }

    private void ChangedEquipmentData(EquipmentType type)
    {
        List<EquipmentData> sortedDatas = _allEquipmentDatas[type];

        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].SetData(sortedDatas[i]);
            _slots[i].UpdateUI(sortedDatas[i]);
            if (sortedDatas[i].isEquipped)
                _slots[i].OnEquippedText(true);
            else
                _slots[i].OnEquippedText(false);

            _slots[i].gameObject.SetActive(_slots[i].Data.IsActive());
        }

        _equipmentPanel.UpdateSlotInfo(sortedDatas[0]);
        _slots[0].SetToggleIsOn();

        IsCombine(type);
    }

    private void OnEquip()
    {
        EquipmentData selectedData = _slots.FirstOrDefault(slot => slot.isSelected)?.Data;
        UpdateEquip(selectedData);
    }

    private void OnUnEquip()
    {
        EquipmentData selectedData = _slots.FirstOrDefault(slot => slot.isSelected)?.Data;
        if (selectedData != null)
        {
            foreach (var slot in _slots)
            {
                if (slot.Data == selectedData)
                {
                    selectedData.isEquipped = false;
                    slot.OnEquippedText(false);
                }
            }
        }

        Managers.GameManager.Player.UnEquip(selectedData.equipmentType);
    }

    private void OnAutoEquip(EquipmentType type)
    {
        List<EquipmentData> datas = GetActiveEquipmentData(type);
        datas.Sort((a, b) => (a.equippedEffect).CompareTo(b.equippedEffect));
        UpdateEquip(datas[datas.Count - 1]);
    }

    private void UpdateEquip(EquipmentData data)
    {
        if (data != null)
        {
            foreach (var slot in _slots)
            {
                if (slot.Data == data)
                {
                    data.isEquipped = true;
                    slot.OnEquippedText(true);
                    slot.SetToggleIsOn();
                }
                else
                {
                    slot.Data.isEquipped = false;
                    slot.OnEquippedText(false);
                }
            }
        }

        Managers.GameManager.Player.Equip(data);
    }

    private void AllCombine(EquipmentType type)
    {
        List<EquipmentData> datas = _allEquipmentDatas[type];

        foreach (var data in datas)
        {
            if (data.quantity >= 4)
            {
                int combineCount = data.quantity / 4;
                data.quantity %= 4;

                EquipmentData nextEquipment = GetNextEquipment(data);
                nextEquipment.quantity += combineCount;
            }
        }

        IsCombine(type);
        ActiveSlots();

        foreach (var slot in _slots)
        {
            if (slot.gameObject.activeSelf)
            {
                slot.SetToggleIsOn(); 
                EquipmentData selectedData = slot.Data;
                _equipmentPanel.UpdateSlotInfo(selectedData);
                break;
            }
        }
    }

    private EquipmentData GetNextEquipment(EquipmentData data)
    {
        List<EquipmentData> datas = _allEquipmentDatas[data.equipmentType];
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i] == data)
                return datas[i + 1];
        }
        return null;
    }

    private void AddPlayerEquipmentData(EquipmentData data)
    {
        data.quantity++;
    }

    private List<EquipmentData> GetActiveEquipmentData(EquipmentType type)
    {
        List<EquipmentData> datas = new List<EquipmentData>();

        foreach (var data in _allEquipmentDatas[type])
        {
            if (data.IsActive())
                datas.Add(data);
        }
        return datas;
    }

    private void ActiveSlots()
    {
        foreach (var slot in _slots)
        {
            slot.gameObject.SetActive(slot.Data.IsActive());
        }
    }

    private void IsCombine(EquipmentType type)
    {
        foreach (var data in _allEquipmentDatas[type])
        {
            if (data.quantity >= 4)
            {
                _equipmentPanel.IsCombineButton(true);
                return;
            }
            else
                _equipmentPanel.IsCombineButton(false);
        }
    }

    private void ShowUpgradePopup()
    {
        EquipmentData selectedData = _slots.FirstOrDefault(slot => slot.isSelected)?.Data;
        _upgradePopup.OpenUI();
        _upgradePopup.UpdateUI(selectedData);
    }

    private void UpgradeEquipment()
    {
        EquipmentData selectedData = _slots.FirstOrDefault(slot => slot.isSelected)?.Data;
        int prevEffect = selectedData.equippedEffect; 
        Managers.CurrencyManager.SubtractCurrency(CurrencyType.UpgradeStone, selectedData.upgradePrice);

        switch (selectedData.equipmentType)
        {
            case EquipmentType.Weapon:
                selectedData.equippedEffect += selectedData.GetUpgradeAmount();
                break;
            case EquipmentType.Armor:
                selectedData.equippedEffect += selectedData.GetUpgradeAmount();
                break;
        }

        if(selectedData.isEquipped)
        {
            Managers.GameManager.Player.UpgradeEquipment(selectedData.equipmentType, prevEffect, selectedData.equippedEffect);
        }

        selectedData.upgradePrice += selectedData.level * 3;
        selectedData.level++;
        _upgradePopup.UpdateUI(selectedData);
    }

    private void UpgradePopupCloseUI()
    {
        var selecteSlot = _slots.FirstOrDefault(slot => slot.isSelected);
        _equipmentPanel.UpdateSlotInfo(selecteSlot.Data);
        selecteSlot.UpdateUI(selecteSlot.Data);
    }
}
