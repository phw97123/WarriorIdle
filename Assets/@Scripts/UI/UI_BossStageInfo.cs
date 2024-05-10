using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossStageInfo : UI_Base
{
    [SerializeField] private Text _name;
    [SerializeField] private Text _hp;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Slider _timerSlider;

    private EnemyDataSO data;

    public void Init(EnemyDataSO data)
    {
        this.data = data;
        _name.text = data.enemyName;
        UpdateHpUI(data.characterData.Hp); 
    }

    public void UpdateHpUI(int hp)
    {
        float progress = (float)hp / data.characterData.MaxHp;
        if (progress< 0)
            progress = 0;

        _hp.text = $"{((float)progress * 100):F2}%";
        _hpSlider.value = (float)progress;
    }

    public void UpdateTimer(float remainingTime)
    {
        _timerSlider.value = (float)remainingTime / 60.0f;
    }
}
