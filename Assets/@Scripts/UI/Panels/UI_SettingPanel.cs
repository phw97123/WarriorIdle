using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_SettingPanel : UI_Base
{
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _effectSlider;
    [SerializeField] private Button _gameExitButton;
    [SerializeField] private Button _closeButton;

    private event Action<float, Define.AudioType> _onChangedVolume;

    private SoundManager _soundManager; 

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _soundManager = Managers.SoundManager; 

        _bgmSlider.onValueChanged.AddListener(OnChangedBgmVolume);
        _effectSlider.onValueChanged.AddListener(OnChangedEffectVolume);
        _gameExitButton.onClick.AddListener(OnClickGameExitButton);
        _closeButton.onClick.AddListener(()=> CloseUI(true));

        _onChangedVolume -= _soundManager.SetVolume; 
        _onChangedVolume += _soundManager.SetVolume;

        OnChangedBgmVolume(0.5f);
        OnChangedEffectVolume(0.5f); 

        return true;
    }

    private void OnChangedBgmVolume(float value)
    {
        _bgmSlider.value = value;
        _onChangedVolume.Invoke(value, Define.AudioType.Bgm); 
    }

    private void OnChangedEffectVolume(float value)
    {
        _effectSlider.value = value;
        _onChangedVolume.Invoke(value, Define.AudioType.Effect); 
    }

    private void OnClickGameExitButton()
    {
        _soundManager.Play(Define.UI_BUTTON); 
        Debug.Log("게임 종료"); 
        Application.Quit(); 
    }
}
