using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DungeonStageInfo : UI_Base
{
    [SerializeField] private Text _name;
    [SerializeField] private Slider _timerSlider;
    [SerializeField] private TMP_Text _takeDamageValue;

    private int _takeDamage; 

    public void SetData(DungeonDataSO data)
    {
        _takeDamage = 0; 
        _name.text = data.dataName;
        _takeDamageValue.text = "입힌 데미지 : 0";
        _timerSlider.value = 1; 
    }

    public void UpdateTimer(float remainingTime, float totalDuration)
    {
        _timerSlider.value = (float)remainingTime / totalDuration;
    }

    public void UpdateDamageText(int value)
    {
        _takeDamage += value; 
        _takeDamageValue.text = "입힌 데미지 : " + _takeDamage.ToString();   
    }
}
