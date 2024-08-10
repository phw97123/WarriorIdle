using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillIconSlot : UI_Base
{
    [SerializeField] private Text _rarityName;
    [SerializeField] private Image _icon;

    private SkillData _currentSkillData;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _currentSkillData = null; 
        _icon.gameObject.SetActive(false);

        return true; 
    }

    public void AssignSkillData(SkillData skillData)
    {
        _currentSkillData = skillData;
        _rarityName.text = $"{Managers.GameManager.GetRarityName(skillData.BaseData.rarity)}";
        _icon.sprite = _currentSkillData.BaseData.icon;
        _icon.gameObject.SetActive(true);
    }

    public void RemoveSkillData()
    {
        _currentSkillData = null; 
        _rarityName.text = string.Empty;
        _icon.sprite = null;
        _icon.gameObject.SetActive(false); 
    }

    public bool IsSkillData()
    {
        return _currentSkillData != null;
    }

    public SkillData GetCurrentData()
    {
        return _currentSkillData;
    }
}
