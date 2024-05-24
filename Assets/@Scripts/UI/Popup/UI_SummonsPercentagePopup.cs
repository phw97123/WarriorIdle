using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;
using static Define; 

public class UI_SummonsPercentagePopup : UI_Base
{
    [SerializeField] private Text[] _raitys;
    [SerializeField] private Text _level; 
    [SerializeField] private Button _closeButton;

    public override bool Init()
    {
        if (base.Init() == false) return false;

        _closeButton.onClick.AddListener(() => CloseUI(true));

        return true; 
    }

    public void UpdateUI(SummonsData data)
    {
        _level.text = $"Lv.{data.level}"; 
        for(int i = 0; i<Enum.GetValues(typeof(Rarity)).Length; i++)
        {
            _raitys[i].text = $"{Managers.GameManager.GetRarityName((Rarity)i)} : {data.SummonsDataSO.Getprobalility(data.level)[i]}%";
        }
    }
}
