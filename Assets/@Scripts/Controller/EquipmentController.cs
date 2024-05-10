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
    private Dictionary<EquipmentType, List<EquipmentData>> _allEquipmentDatas;
    private UI_EquipmentPanel _equipmentPanel;
    private UI_UpgradePopup _upgradePopup;
    private List<UI_EquipmentSlot> _slots;
    private int _rarityMaxLevel = 4;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _allEquipmentDatas = new Dictionary<EquipmentType, List<EquipmentData>>();
        _slots = new List<UI_EquipmentSlot>(24);

        CreateAllWeapon();
        CreateAllArmor();

        AddPlayerEquipmentData(_allEquipmentDatas[EquipmentType.Weapon][0]);
        AddPlayerEquipmentData(_allEquipmentDatas[EquipmentType.Armor][0]);
        AddPlayerEquipmentData(_allEquipmentDatas[EquipmentType.Weapon][1]);
        AddPlayerEquipmentData(_allEquipmentDatas[EquipmentType.Weapon][2]);
        AddPlayerEquipmentData(_allEquipmentDatas[EquipmentType.Weapon][3]);
        AddPlayerEquipmentData(_allEquipmentDatas[EquipmentType.Weapon][4]);
        AddPlayerEquipmentData(_allEquipmentDatas[EquipmentType.Weapon][5]);
        AddPlayerEquipmentData(_allEquipmentDatas[EquipmentType.Weapon][5]);
        AddPlayerEquipmentData(_allEquipmentDatas[EquipmentType.Weapon][5]);
        AddPlayerEquipmentData(_allEquipmentDatas[EquipmentType.Weapon][5]);
        AddPlayerEquipmentData(_allEquipmentDatas[EquipmentType.Weapon][5]);

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

        _upgradePopup.UpgradeButtonInjection(UpgradeEquipment);
        _upgradePopup.CloseButtonInjection(UpgradePopupCloseUI);

        return true;
    }

    private void CreateAllWeapon()
    {
        int prevEffect = 0;
        int rarityEffect = 1;
        List<EquipmentData> weaponDatas = new List<EquipmentData>();
        foreach (Rarity rarity in Enum.GetValues(typeof(Rarity)))
        {
            for (int rarityLevel = 1; rarityLevel <= _rarityMaxLevel; rarityLevel++)
            {
                string fileName = $"Weapon{rarity}{rarityLevel}.sprite";
                string name = GetRarityName(rarity);
                Sprite icon = Managers.ResourceManager.Load<Sprite>(fileName);
                int equippedEffect = 3 + prevEffect + rarityEffect;
                int upgradeStone = 150 + prevEffect + rarityEffect;

                EquipmentData data = new EquipmentData(name, icon, rarityLevel, EquipmentType.Weapon, rarity, equippedEffect, upgradeStone);
                weaponDatas.Add(data);
                prevEffect = equippedEffect;
            }
            rarityEffect += 6;
        }
        _allEquipmentDatas.Add(EquipmentType.Weapon, weaponDatas);
    }

    private void CreateAllArmor()
    {
        int prevEffect = 0;
        int rarityEffect = 1;
        List<EquipmentData> armorDatas = new List<EquipmentData>();
        foreach (Rarity rarity in Enum.GetValues(typeof(Rarity)))
        {
            for (int rarityLevel = 1; rarityLevel <= _rarityMaxLevel; rarityLevel++)
            {
                string fileName = $"Armor{rarity}{rarityLevel}.sprite";
                string name = GetRarityName(rarity);
                Sprite icon = Managers.ResourceManager.Load<Sprite>(fileName);
                int equippedEffect = 1 + prevEffect + rarityEffect;
                int upgradeStone = 150 + prevEffect + rarityEffect;

                EquipmentData data = new EquipmentData(name, icon, rarityLevel, EquipmentType.Armor, rarity, equippedEffect, upgradeStone);
                armorDatas.Add(data);
                prevEffect = equippedEffect;
            }
            rarityEffect += 3;
        }
        _allEquipmentDatas.Add(EquipmentType.Armor, armorDatas);
    }

    private void CreateSlots(Transform parent)
    {
        // TODO : 플레이어 데이터에 맞게 슬롯 활성화 및 비활성화 
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
        // TODO : PlayerData로 변경
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
        Managers.CurrencyManager.SubtractCurrency(CurrencyType.UpgradeStone, selectedData.upgradeStone);

        switch (selectedData.equipmentType)
        {
            case EquipmentType.Weapon:
                selectedData.equippedEffect += 3;
                break;
            case EquipmentType.Armor:
                selectedData.equippedEffect += 1;
                break;
        }

        if(selectedData.isEquipped)
        {
            Managers.GameManager.Player.UpgradeEquipment(selectedData.equipmentType, prevEffect, selectedData.equippedEffect);
        }

        selectedData.upgradeStone += selectedData.level * 3;
        selectedData.level++;
        _upgradePopup.UpdateUI(selectedData);
    }

    private void UpgradePopupCloseUI()
    {
        var selecteSlot = _slots.FirstOrDefault(slot => slot.isSelected);
        _equipmentPanel.UpdateSlotInfo(selecteSlot.Data);
        selecteSlot.UpdateUI(selecteSlot.Data);
    }

    public string GetRarityName(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return "일반";
            case Rarity.Rare:
                return "레어";
            case Rarity.Epic:
                return "에픽";
            case Rarity.Ancient:
                return "영웅";
            case Rarity.Legendary:
                return "전설";
            case Rarity.MyThology:
                return "신화";
        }
        return null;
    }
}
