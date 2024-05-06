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
        UpdateHpUI(data.characterData.HP); 
    }

    public void UpdateHpUI(int hp)
    {
        float progress = Mathf.Min((float)hp / data.characterData.maxHp, 0.0f);
        _hp.text = $"{((float)progress * 100):F2}%";
        _hpSlider.value = (float)progress;
    }

    public void UpdateTimer(float remainingTime)
    {
        _timerSlider.value = (float)remainingTime / 60.0f;
    }
}
