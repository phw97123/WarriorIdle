using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_SettingPanel : UI_Base
{
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _effectSlider;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _deleteDataButton;

    private event Action<float, Define.AudioType> _onChangedVolume;

    private SoundManager _soundManager; 

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _soundManager = Managers.SoundManager; 

        _bgmSlider.onValueChanged.AddListener(OnChangedBgmVolume);
        _effectSlider.onValueChanged.AddListener(OnChangedEffectVolume);
        _closeButton.onClick.AddListener(()=> CloseUI(true));
        _deleteDataButton.onClick.AddListener(OnDeleteData);

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

    private void OnDeleteData()
    {
        CloseUI(false);
        Managers.DataManager.DeleteAllData();
        Managers.DestroyManagers(); 
        StopAllCoroutines();
        SceneManager.LoadScene(Define.SceneType.StartScene.ToString()); 
    }
}
