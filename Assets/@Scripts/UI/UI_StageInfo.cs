using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageInfo : UI_Base
{
    [SerializeField] private Text _stageIndex;
    [SerializeField] private Text _stageExpText;
    [SerializeField] private Slider _stageExpSlider;
    [SerializeField] private Button _tryBossButton;

    private SoundManager _soundManager;
    private Action _onClickTryBossButton;

    private GameManager _gameManager;

    private int _nextStageEnemyCount;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _soundManager = Managers.SoundManager; 
        _gameManager = Managers.GameManager;
        _tryBossButton.onClick.AddListener(OnClickTryBossButton);

        return true; 
    }

    public void TryBossButtonInjection(Action onClickTryBossButton)
    {
        _onClickTryBossButton = onClickTryBossButton;
    }

    private void OnClickTryBossButton()
    {
        _soundManager.Play(Define.UI_BUTTON);

        _onClickTryBossButton.Invoke();
    }

    public void UpdateUI(StageData data)
    {
        _stageIndex.text = data.stageName;
        _nextStageEnemyCount = data.nextStageEnemyCount;
    }

    public void UpdateStageExp(int killCount)
    {
        float progress = Mathf.Min((float)killCount / _nextStageEnemyCount, 1.0f); 
        _stageExpText.text = $"{progress * 100:F2}%";
        _stageExpSlider.value = progress;

        if (_nextStageEnemyCount <= killCount)
            _tryBossButton.interactable = true;
        else
            _tryBossButton.interactable = false;
    }
}
