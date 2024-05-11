using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StartScreen : UI_Base
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Image _touchImage;

    private Action _onClickStartButton;

    private float _blinkInterval = 1.0f;
    private bool _isBlinking = false;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _startButton.onClick.AddListener(OnClickStartButton);

        return false;
    }

    public void Start()
    {
        StartBlinking(); 
    }

    public void StartBlinking()
    {
        if (!_isBlinking)
        {
            _isBlinking = true;
            StartCoroutine(COBlinking()); 
        }
    }

    public void StopBlinking()
    {
        if(_isBlinking)
        {
            _isBlinking = false; 
            StopCoroutine(COBlinking());
        }
    }

    private IEnumerator COBlinking()
    {
        while (_isBlinking)
        {
            _touchImage.enabled = !_touchImage.enabled;
            yield return new WaitForSeconds(_blinkInterval);
        }
    }

    private void OnClickStartButton()
    {
        StopBlinking(); 
        _onClickStartButton?.Invoke();
    }

    public void StartButtonInjection(Action action)
    {
        _onClickStartButton = action;
    }
}
