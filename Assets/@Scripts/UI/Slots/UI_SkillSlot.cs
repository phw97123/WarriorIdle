using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSlot : UI_Base
{
    [SerializeField] private Text _rarityName;
    [SerializeField] private Text _upgradeLevel;
    [SerializeField] private Image _icon;
    [SerializeField] private Toggle _selectToggle;
    [SerializeField] private GameObject _equippedText;

    private Action<SkillData> _onShowDataInfo;
    public bool isSelected = false;

    private SkillData _data; 
    public SkillData Data { get { return _data; } }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _selectToggle.onValueChanged.AddListener((isOn) =>
        {
            Managers.SoundManager.Play(Define.UI_BUTTON);

            if (!isOn) isSelected = false;
            else
            {
                isSelected = true;
                OnShowDataInfo();
            }
        });

        return true; 
    }

    public void UpdateUI(SkillData data)
    {
        _rarityName.text = $"{Managers.DataManager.GetRarityName(data.baseData.rarity)}";
        _upgradeLevel.text = $"Lv.{data.level}";
        _icon.sprite = data.baseData.icon;
        _equippedText.SetActive(data.isEquipped);
    }

    public void OnShowDataInfo()
    {
        _onShowDataInfo?.Invoke(Data); 
    }
    
    public void SetData(SkillData data)
    {
        _data = data; 
    }

    public void SetToggleIsOn()
    {
        _selectToggle.isOn = true;
    }

    public void ShowDataInfoInjection(Action<SkillData> action)
    {
        _onShowDataInfo = action;
    }
}
