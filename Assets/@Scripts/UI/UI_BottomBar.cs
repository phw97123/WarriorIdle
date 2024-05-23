using UnityEngine;
using UnityEngine.UI;

public class UI_BottomBar : UI_Base
{
    [SerializeField] private Toggle _growth;
    [SerializeField] private Toggle _shop;
    [SerializeField] private Toggle _summons;
    [SerializeField] private Toggle _equipment;
    [SerializeField] private Toggle _skill;
    [SerializeField] private Toggle _dungeon;

    private UI_GrowthPanel _growthPanel;
    private UI_ShopPanel _shopPanel;
    private UI_SummonsPanel _summonsPanel;
    private UI_EquipmentPanel _equipmentPanel;
    private UI_SkillPanel _skillPanel;
    private UI_DungeonPanel _dungeonPanel;

    private UIManager _uiManager;

    private GrowthController _controller;

    public override bool Init()
    {
        if (base.Init() == false) return false;

        _uiManager = Managers.UIManager;

        _uiManager.TryGetUIComponent(out _growthPanel);
        _uiManager.TryGetUIComponent(out _shopPanel);
        _uiManager.TryGetUIComponent(out _summonsPanel);
        _uiManager.TryGetUIComponent(out _equipmentPanel);
        _uiManager.TryGetUIComponent(out _skillPanel);
        _uiManager.TryGetUIComponent(out _dungeonPanel);

        _growth.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, _growthPanel));
        _shop.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, _shopPanel));
        _summons.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, _summonsPanel));
        _equipment.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, _equipmentPanel));
        _skill.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, _skillPanel));
        _dungeon.onValueChanged.AddListener((isOn) => OnToggleChanged(isOn, _dungeonPanel));

        return true;
    }

    private void OnToggleChanged<T>(bool isOn, T panel) where T : UI_Base
    {
        Managers.SoundManager.Play(Define.UI_BUTTON); 

        if (isOn)
            panel.OpenUI();
        else 
            panel.CloseUI();
    }
}
