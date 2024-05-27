using UnityEngine;
using UnityEngine.UI;

public class UI_RewardSlot : UI_Base
{
    [SerializeField] private Image _icon;
    [SerializeField] private Text _value;

    public void SetData(RewardData data)
    {
        _icon.sprite = data.Icon;
        _value.text = $"+{data.Value}";
    }
}
