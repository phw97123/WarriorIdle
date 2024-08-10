using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class SkillController : BaseController
{
    private Dictionary<SkillType, List<SkillData>> _allSkillData { get { return Managers.GameManager?.AllSkillDatas; } }
    private UI_SkillPanel _skillPanel;
    private UI_UpgradePopup _upgradePopup;
    private UI_SkillBar _skillBar;

    private List<UI_SkillSlot> _slots;

    private Dictionary<string, Type> _skillTypeMapping;
    private Dictionary<string, SkillBase> _skills;

    public Action OnCreateData;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _slots = new List<UI_SkillSlot>();
        _skills = new Dictionary<string, SkillBase>();
        SkillMapping();

        #region TestCode
        //foreach (var key in _allSkillData.Keys)
        //{
        //    List<SkillData> skillDatas = _allSkillData[key];
        //    foreach (var data in skillDatas)
        //    {
        //        AddPlayerSkillData(data);
        //    }
        //}
        #endregion

        Managers.GameManager.OnAddSkillData = AddPlayerSkillData; 

        AddPlayerSkillData(_allSkillData[SkillType.Active][0]);
        AddPlayerSkillData(_allSkillData[SkillType.Passive][0]);

        Managers.UIManager.TryGetUIComponent(out _skillPanel);
        Managers.UIManager.TryGetUIComponent(out _upgradePopup);
        Managers.UIManager.TryGetUIComponent(out _skillBar);

        _upgradePopup.gameObject.SetActive(false);

        _skillPanel.CreateSlotsInjection(CreateSlots);
        _skillPanel.ChangedTabInjection(ChangedSkillData);
        _skillPanel.OpenUIUpdate(PanelUpdateUI);
        _skillPanel.ClickAllDismantleButtonInjection(AllDisMantle);
        _skillPanel.ClickUpgradeButtonInjection(ShowUpgradePopup);
        _skillPanel.ClickEquipButtonInjection(OnEquip);
        _skillPanel.ClickUnEquipButtonInjection(OnUnEquip);

        _upgradePopup.UpgradeButtonInjection(UpgradeSkill, PopupType.Skill);
        _upgradePopup.CloseButtonInjection(UpgradePopupCloseUI, PopupType.Skill);

        foreach (var skillButton in _skillBar.GetSkillButtons())
        {
            skillButton.OnClickExecuteButtonInjection(OnSkillExecute);
        }

        return true;
    }

    private void SkillMapping()
    {
        _skillTypeMapping = new Dictionary<string, Type>
        {
            { "Explosion", typeof(Explosion) },
            { "Electric", typeof(Electric) },
            { "HPRegeneration", typeof(HPRegeneration)},
            { "MPRegeneration", typeof(MPRegeneration)}
        };
    }

    private void PanelUpdateUI()
    {
        SkillData data = null;

        foreach (var slot in _slots)
        {
            if (slot.isSelected)
            {
                data = slot.Data;
                break;
            }
        }
        UpdateSlotsForAllSkillTypes(); 
        _skillPanel.UpdateSlotInfo(_slots[0].Data);
    }

    private void CreateSlots(Transform parent)
    {
        Dictionary<SkillType, List<SkillData>> allDatas = _allSkillData;

        foreach (var key in allDatas.Keys)
        {
            List<SkillData> skillDatas = allDatas[key];
            foreach (var data in skillDatas)
            {
                GameObject go = Managers.ResourceManager.Instantiate(UISKILLSLOT_PREFAB, parent);
                var slot = go.GetOrAddComponent<UI_SkillSlot>();
                slot.transform.SetParent(parent, false);
                _slots.Add(slot);

                Toggle toggle = slot.GetComponentInChildren<Toggle>();
                toggle.group = parent.GetComponent<ToggleGroup>();

                slot.ShowDataInfoInjection(_skillPanel.UpdateSlotInfo);
                slot.gameObject.SetActive(data.IsActive());
                slot.UpdateUI(data);
            }
        }
    }

    private void ChangedSkillData(SkillType type)
    {
        if (type == SkillType.None)
        {
            UpdateSlotsForAllSkillTypes();
        }
        else
        {
            UpdateSlotsForSpecificSkillType(type);
        }

        _skillPanel.UpdateSlotInfo(_slots[0].Data);
        _slots[0].SetToggleIsOn();
    }

    private void UpdateSlotsForAllSkillTypes()
    {
        int slotCount = 0;
        foreach (var key in _allSkillData.Keys)
        {
            List<SkillData> sortedDatas = _allSkillData[key];
            foreach (var data in sortedDatas)
            {
                UpdateSlotData(slotCount, data);
                slotCount++;
            }
        }
    }

    private void UpdateSlotsForSpecificSkillType(SkillType type)
    {
        List<SkillData> sortedDatas = _allSkillData[type];
        for (int i = 0; i < _slots.Count; i++)
        {
            if (i >= sortedDatas.Count)
            {
                _slots[i].gameObject.SetActive(false);
                continue;
            }
            UpdateSlotData(i, sortedDatas[i]);
        }
    }

    private void UpdateSlotData(int slotIndex, SkillData data)
    {
        _slots[slotIndex].SetData(data);
        _slots[slotIndex].UpdateUI(data);
        _slots[slotIndex].gameObject.SetActive(data.IsActive());
    }

    private void AllDisMantle(SkillType type)
    {
        List<SkillData> datas = _allSkillData[type];

        foreach (var data in datas)
        {
            if (data.quantity >= 4)
            {
                int dismantleCount = data.quantity - 1;
                data.quantity = 1;

                Managers.CurrencyManager.AddCurrency(CurrencyType.SkillUpgradeStone, dismantleCount * data.BaseData.dismantalRewardCount);
            }
        }

        PanelUpdateUI();
    }

    private void ShowUpgradePopup()
    {
        SkillData selectedData = _slots.FirstOrDefault(slot => slot.isSelected)?.Data;
        _upgradePopup.OpenUI();
        _upgradePopup.UpdateUI(selectedData);
    }

    private void UpgradeSkill()
    {
        SkillData selectedData = _slots.FirstOrDefault(slot => slot.isSelected)?.Data;
        Managers.CurrencyManager.SubtractCurrency(CurrencyType.SkillUpgradeStone, selectedData.upgradePrice);

        selectedData.LevelUp();
        _upgradePopup.UpdateUI(selectedData);
    }

    private void UpgradePopupCloseUI()
    {
        var selecteSlot = _slots.FirstOrDefault(slot => slot.isSelected);
        _skillPanel.UpdateSlotInfo(selecteSlot.Data);
        selecteSlot.UpdateUI(selecteSlot.Data);
    }

    private void OnEquip()
    {
        foreach (var slot in _slots)
        {
            if (slot.isSelected)
            {
                slot.Data.isEquipped = true;
                slot.UpdateUI(slot.Data);
                _skillPanel.AddSkillSlot(slot.Data);
                _skillBar.AddSkillButtonData(slot.Data);
                return;
            }
        }
    }

    private void OnUnEquip()
    {
        foreach (var slot in _slots)
        {
            if (slot.isSelected)
            {
                slot.Data.isEquipped = false;
                slot.UpdateUI(slot.Data);
                _skillPanel.RemoveSkillData(slot.Data);
                _skillBar.RemoveSkillButton(slot.Data);
                return;
            }
        }
    }

    public void OnSkillExecute(SkillData data)
    {
        if (Managers.GameManager.Player.isDead) return;
        if (Managers.GameManager.Player.PlayerData.MP < data.BaseData.mpCost) return;

        Managers.GameManager.Player.PlayerData.MP -= data.BaseData.mpCost;
        UseSkill(data);
    }

    public void UseSkill(SkillData data)
    {
        if (data.CanUseSkill())
        {
            data.lastUsedTime = Time.time;

            if (_skills.TryGetValue(data.BaseData.name, out var skill))
            {
                skill.Execute(data);
            }
        }
        else
        {
            Debug.Log("Skill is on cooldown.");
        }
    }

    public void AddPlayerSkillData(SkillData data)
    {
        data.quantity++;

        if (_skillTypeMapping.TryGetValue(data.BaseData.name, out Type skillType))
        {
            AddSkill(skillType, data);
        }
    }

    private void AddSkill(Type skillType, SkillData data)
    {
        if (_skills.ContainsKey(data.BaseData.name)) return;

        var skill = (SkillBase)gameObject.AddComponent(skillType);
        _skills.Add(data.BaseData.name, skill);

        if (data.BaseData.skillType == SkillType.Passive)
            skill.Execute(data);
    }
}
