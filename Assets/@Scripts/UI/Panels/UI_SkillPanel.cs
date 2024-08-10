using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_SkillPanel : UI_Base
{
    [SerializeField] private List<UI_SkillIconSlot> _equippedSkillSlots;

    [SerializeField] private Toggle _allTab;
    [SerializeField] private Toggle _activeTab;
    [SerializeField] private Toggle _passiveTab;

    [SerializeField] private Image _icon;
    [SerializeField] private Text _equipmentName;
    [SerializeField] private Text _upgradeLevel;
    [SerializeField] private Text _equipEffectText;
    [SerializeField] private Slider _quantitySlider;
    [SerializeField] private Text _quantityText;

    [SerializeField] private Button _equipButton;
    [SerializeField] private Button _unEquipButton;
    [SerializeField] private Button _alldismantleButton;
    [SerializeField] private Button _upgradeButton;

    [SerializeField] private Text _skillUpgradeStone;
    [SerializeField] private Transform _slotParent;

    private Action _onOpenUI;
    private Action<Transform> _onCreateSlots;
    private Action<SkillType> _onChangedTab;
    private Action<SkillType> _onClickAllDismantleButton;
    private Action _onClickUpgrade;
    private Action _onClickEquipped;
    private Action _onClickUnEquipped;

    private SkillType _tabType;

    private SoundManager _soundManager;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _soundManager = Managers.SoundManager;

        OnCreateSlots();

        _tabType = SkillType.None;
        _allTab.onValueChanged.AddListener((isOn) => OnChangedTab(SkillType.None));
        _activeTab.onValueChanged.AddListener((isOn) => OnChangedTab(SkillType.Active));
        _passiveTab.onValueChanged.AddListener((isOn) => OnChangedTab(SkillType.Passive));
        _alldismantleButton.onClick.AddListener(() => OnClickAllDismantleButton(_tabType));
        _upgradeButton.onClick.AddListener(OnClickUpgradeButton);

        _equipButton.onClick.AddListener(OnClickEquipButton);
        _unEquipButton.onClick.AddListener(OnClickUnEquipButton);

        return true;
    }

    public override void OpenUI()
    {
        base.OpenUI();
        _allTab.isOn = true;
        _onOpenUI.Invoke();
    }

    private void OnCreateSlots()
    {
        _onCreateSlots?.Invoke(_slotParent);
    }

    private void OnChangedTab(SkillType type)
    {
        _soundManager.Play(UI_BUTTON);

        _tabType = type;
        _onChangedTab?.Invoke(type);
    }

    private void OnClickAllDismantleButton(SkillType type)
    {
        _soundManager.Play(UI_BUTTON);

        if (_tabType == SkillType.None)
        {
            _onClickAllDismantleButton?.Invoke(SkillType.Active);
            _onClickAllDismantleButton?.Invoke(SkillType.Passive);
        }
        else
        {
            _onClickAllDismantleButton?.Invoke(type);
        }
    }

    public void UpdateSlotInfo(SkillData data)
    {
        _icon.sprite = data.BaseData.icon;
        _upgradeLevel.text = $"Lv.{data.level}";
        _equipmentName.text = $"{Managers.GameManager.GetRarityName(data.BaseData.rarity)}";
        
        _equipEffectText.text = $"{data.BaseData.skillName}  [MP:{data.BaseData.mpCost}/{data.BaseData.cool}ÃÊ]\n-{data.BaseData.discription}";
        _quantitySlider.value = (float)data.quantity / 4;
        _quantityText.text = $"{data.quantity}/4";

        _alldismantleButton.interactable = data.quantity >= 4;

        SkillUpgradeStoneUpdate();

        UpdateEquipButton(data.isEquipped);

        if(IsEmptySkillSlot() && data.BaseData.skillType == SkillType.Active)
            _equipButton.interactable = true;
        else
            _equipButton.interactable = false;
    }

    public void SkillUpgradeStoneUpdate()
    {
        _skillUpgradeStone.text = Managers.CurrencyManager.GetCurrencyAmount(CurrencyType.SkillUpgradeStone);
    }

    private void OnClickUpgradeButton()
    {
        _soundManager.Play(UI_BUTTON);

        _onClickUpgrade?.Invoke();
    }

    private void OnClickEquipButton()
    {
        if(IsEmptySkillSlot())
        {
            _soundManager.Play(UI_BUTTON);

            _onClickEquipped.Invoke();
            UpdateEquipButton(true);
        }
    }

    private void OnClickUnEquipButton()
    {
        _soundManager.Play(UI_BUTTON);

        _onClickUnEquipped.Invoke();
        UpdateEquipButton(false);
    }

    private void UpdateEquipButton(bool isEquipped)
    {
        _equipButton.gameObject.SetActive(!isEquipped);
        _unEquipButton.gameObject.SetActive(isEquipped);
    }

    public void AddSkillSlot(SkillData data)
    {
        foreach (var slot in _equippedSkillSlots)
        {
            if (!slot.IsSkillData())
            {
                slot.AssignSkillData(data);
                return;
            }
        }
    }

    public void RemoveSkillData(SkillData data)
    {
        foreach(var slot in _equippedSkillSlots)
        {
            if(slot.GetCurrentData() == data)
            {
                slot.RemoveSkillData();
                return;
            }
        }
    }

    public bool IsEmptySkillSlot()
    {
        foreach(var slot in _equippedSkillSlots)
        {
            if(!slot.IsSkillData())
                return true;
        }
        return false;
    }

    #region Injection
    public void OpenUIUpdate(Action action)
    {
        _onOpenUI = action;
    }
    public void CreateSlotsInjection(Action<Transform> action)
    {
        _onCreateSlots = action;
    }

    public void ChangedTabInjection(Action<SkillType> action)
    {
        _onChangedTab = action;
    }

    public void ClickAllDismantleButtonInjection(Action<SkillType> action)
    {
        _onClickAllDismantleButton = action;
    }

    public void ClickUpgradeButtonInjection(Action action)
    {
        _onClickUpgrade = action;
    }

    public void ClickEquipButtonInjection(Action action)
    {
        _onClickEquipped = action;
    }

    public void ClickUnEquipButtonInjection(Action action)
    {
        _onClickUnEquipped = action;
    }
    #endregion
}
