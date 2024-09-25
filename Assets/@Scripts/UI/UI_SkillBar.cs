using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillBar : UI_Base
{
    [SerializeField] private List<UI_SkillButton> _skillButtons;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        List<SkillData> skillData = Managers.GameManager.skillDataCollection.SkillDataList;

        foreach (var data in skillData)
        {
            if (data.isEquipped)
            {
                _skillButtons[data.slotIndex].UpdateData(data);
            }
        }

        return true;
    }

    public void AddSkillButtonData(SkillData data)
    {
        for (int i = 0; i < _skillButtons.Count; i++)
        {
            if (_skillButtons[i].GetCurrentData() == null)
            {
                _skillButtons[i].UpdateData(data);
                data.slotIndex = i;
                return;
            }
        }
    }

    public void RemoveSkillButton(SkillData data)
    {
        foreach (var button in _skillButtons)
        {
            if (button.GetCurrentData() == data)
            {
                button.UpdateData(null);
              
                data.slotIndex = -1;
                return;
            }
        }
    }

    public List<UI_SkillButton> GetSkillButtons()
    {
        return _skillButtons;
    }
}
