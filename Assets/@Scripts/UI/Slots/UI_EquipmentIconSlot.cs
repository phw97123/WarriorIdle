using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipmentIconSlot : UI_Base
{
    [SerializeField] private Text _name;
    [SerializeField] private Image _icon;

    public void UpdateUI(EquipmentData data)
    {
        _name.text = $"{data.rarityName}{data.level}";
        _icon.sprite = data.icon; 
    }
}
