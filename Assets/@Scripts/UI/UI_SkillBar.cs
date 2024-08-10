using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillBar : UI_Base
{
    [SerializeField] private List<UI_SkillButton> _skillButtons;

    public void AddSkillButtonData(SkillData data)
    {
        foreach (var button in _skillButtons)
        {
            if (button.GetCurrentData() == null)
            {
                button.UpdateData(data);
                return;
            }
        }
    }

    public void RemoveSkillButton(SkillData data)
    {
        foreach (var button in _skillButtons)
        {
            if(button.GetCurrentData() == data)
            {
                button.UpdateData(null);
                return;
            }
        }
    }

    public List<UI_SkillButton> GetSkillButtons()
    {
        return _skillButtons;
    }
}
