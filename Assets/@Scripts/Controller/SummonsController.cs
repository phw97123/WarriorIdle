using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class SummonsController : BaseController
{
    private SoundManager _soundManager;

    private List<SummonsData> datas;

    private UI_SummonsPanel _summonsPanel;
    private UI_SummonsPopup _summonsPopup;
    private UI_SummonsPercentagePopup _percentagePopup;
    private List<EquipmentData> _summonEquipmentDatas;
    private List<UI_EquipmentIconSlot> _equipmentIconSlots;

    private List<SkillData> _summonSkillDatas;
    private List<UI_SkillIconSlot> _skillIconSlots;

    private WaitForSeconds _waitForDrawSlot;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _soundManager = Managers.SoundManager;

        datas = new List<SummonsData>(3);
        _summonEquipmentDatas = new List<EquipmentData>();
        _equipmentIconSlots = new List<UI_EquipmentIconSlot>();
        _summonSkillDatas = new List<SkillData>();
        _skillIconSlots = new List<UI_SkillIconSlot>();

        _waitForDrawSlot = new WaitForSeconds(0.05f);

        datas = Managers.DataManager.summonsDataCollection.summonsDataList; 

        Managers.UIManager.TryGetUIComponent(out _summonsPanel);
        _summonsPanel.CreatedSlotsInjection(CreateSlots);

        Managers.UIManager.TryGetUIComponent(out _summonsPopup);
        _summonsPopup.gameObject.SetActive(false);
        _summonsPopup.CloseUIInjection(IconSlotDestroy);

        Managers.UIManager.TryGetUIComponent(out _percentagePopup);
        _percentagePopup.gameObject.SetActive(false);

        return true;
    }

    private void CreateSlots(Transform parent)
    {
        foreach (var data in datas)
        {
            GameObject go = Managers.ResourceManager.Instantiate(UISUMMONSSLOT_PREFAB, parent);
            var slot = go.GetOrAddComponent<UI_SummonsSlot>();
            slot.transform.SetParent(parent, false);

            if (data.summonsDataSO.type == SummonsType.Skill)
                slot.SummonsButtonInjection(ShowSummonsSkillPopup);
            else
                slot.SummonsButtonInjection(ShowSummonsEquipmentPopup);

            slot.PercentageViewInjection(ShowPercentageView);

            slot.SetUI(data);

            data.onExpChanged -= slot.UpdateUI;
            data.onExpChanged += slot.UpdateUI;

            data.onLevelChanged -= slot.UpdateUI;
            data.onExpChanged += slot.UpdateUI;
        }
    }

    private void ShowSummonsEquipmentPopup(int count, SummonsType type)
    {
        _summonsPopup.OpenUI();
        SummonsEquipment(count, type, _summonsPopup.GetParent());
    }

    private void ShowSummonsSkillPopup(int count, SummonsType type)
    {
        _summonsPopup.OpenUI();
        SummonsSkill(count, type, _summonsPopup.GetParent());
    }

    private void SummonsEquipment(int count, SummonsType type, Transform parent)
    {
        _summonEquipmentDatas.Clear();
        SummonsData data = datas[(int)type];

        Managers.CurrencyManager.SubtractCurrency(CurrencyType.Dia, data.Price);
        data.AddExp(count * 10);

        List<float> probabilityArray = data.summonsDataSO.Getprobalility(data.level);
        for (int i = 0; i < count; i++)
        {
            Rarity selectedRarity = GetRandomRarity(probabilityArray);
            int rarityLevel = Random.Range(1, 5);
            EquipmentType equipmentType = GetEquipementType(type);
            string typeName = $"{type}{selectedRarity}{rarityLevel}";

            EquipmentData summonsData = Managers.DataManager.GetEquipmentData(equipmentType, typeName);
            summonsData.quantity++;
            _summonEquipmentDatas.Add(summonsData);
        }

        CreateEquipmentIconSlots(parent);
    }

    private void CreateEquipmentIconSlots(Transform parent)
    {
        _equipmentIconSlots.Clear();

        foreach (var data in _summonEquipmentDatas)
        {
            GameObject go = Managers.ResourceManager.Instantiate(UIEQUIPMENTICONSLOT_PREFAB, parent, true);
            UI_EquipmentIconSlot slot = go.GetOrAddComponent<UI_EquipmentIconSlot>();
            slot.transform.SetParent(parent, false);
            slot.UpdateUI(data);
            slot.gameObject.SetActive(false);
            _equipmentIconSlots.Add(slot);
        }
        StartCoroutine(DrawSlots(_equipmentIconSlots));
    }

    private void SummonsSkill(int count, SummonsType type, Transform parent)
    {
        _summonSkillDatas.Clear();
        SummonsData data = datas[(int)type];

        Managers.CurrencyManager.SubtractCurrency(CurrencyType.Dia, data.Price);
        data.AddExp(count * 10);

        List<float> probabilityArray = data.summonsDataSO.Getprobalility(1);
        for (int i = 0; i < count; i++)
        {
            Rarity selectedRarity = GetRandomRarity(probabilityArray);
            List<SkillData> raritySkillDatas = new List<SkillData>();
            foreach (var skillType in Managers.DataManager.AllSkillDatas.Keys)
            {
                foreach (var skilldata in Managers.DataManager.AllSkillDatas[skillType])
                {
                    if (skilldata.baseData.rarity == selectedRarity)
                    {
                        raritySkillDatas.Add(skilldata);
                    }
                }
            }

            int randNum = Random.Range(0, raritySkillDatas.Count);
            SkillData summonsData = raritySkillDatas[randNum];
            Managers.DataManager.AddSkillData(summonsData);
            _summonSkillDatas.Add(summonsData);
        }

        CreateSkillIconSlots(parent);
    }

    private void CreateSkillIconSlots(Transform parent)
    {
        _skillIconSlots.Clear();

        foreach (var data in _summonSkillDatas)
        {
            GameObject go = Managers.ResourceManager.Instantiate("UI_SkillIconSlot.prefab", parent, true);
            UI_SkillIconSlot slot = go.GetOrAddComponent<UI_SkillIconSlot>();
            slot.transform.SetParent(parent, false);
            slot.UpdateSkillData(data);
            slot.gameObject.SetActive(false);
            _skillIconSlots.Add(slot);
        }
        StartCoroutine(DrawSlots(_skillIconSlots));
    }

    private IEnumerator DrawSlots<T>(List<T> slots) where T : UI_Base
    {
        _summonsPopup.CloseButtonInteractable(false);

        if (slots.Count > 0)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].gameObject.SetActive(true);
                yield return _waitForDrawSlot;
            }
        }
        _soundManager.Play(UPGRADE);

        _summonsPopup.CloseButtonInteractable(true);

        yield return null;
    }

    private void IconSlotDestroy()
    {
        if (_skillIconSlots.Count > 0)
        {
            _skillIconSlots.Reverse();
            foreach (var slot in _skillIconSlots)
            {
                if (slot.gameObject.activeSelf)
                {
                    Managers.ResourceManager.Destroy(slot.gameObject);
                }
            }
        }

        if (_equipmentIconSlots.Count > 0)
        {
            _equipmentIconSlots.Reverse();
            foreach (var slot in _equipmentIconSlots)
            {
                if (slot.gameObject.activeSelf)
                {
                    Managers.ResourceManager.Destroy(slot.gameObject);
                }
            }
        }
    }

    private Rarity GetRandomRarity(List<float> probabilityArray)
    {
        float randomValue = Random.value * 100f;
        float cumulativeProbability = 0f;

        for (int i = 0; i < probabilityArray.Count; i++)
        {
            cumulativeProbability += probabilityArray[i];
            if (randomValue < cumulativeProbability)
                return (Rarity)i;
        }
        return Rarity.Common;
    }

    private EquipmentType GetEquipementType(SummonsType type)
    {
        switch (type)
        {
            case SummonsType.Weapon:
                return EquipmentType.Weapon;
            case SummonsType.Armor:
                return EquipmentType.Armor;
        }
        return EquipmentType.Weapon;
    }

    private void ShowPercentageView(SummonsData data)
    {
        _percentagePopup.OpenUI();
        _percentagePopup.UpdateUI(data);
    }
}
