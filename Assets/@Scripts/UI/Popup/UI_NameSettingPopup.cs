using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_NameSettingPopup : UI_Base
{
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private Button _checkButton;
    private string _playerName = null;
    private UI_BasePopup _basePopup;

    private void Start()
    {
        _playerName = _nameInput.text; 
        Managers.UIManager.TryGetUIComponent(out  _basePopup);
        _checkButton.onClick.AddListener(OnClickCheckButton); 
    }
    
    private void OnClickCheckButton()
    {
        _playerName = _nameInput.text;
        string check = Regex.Replace(_playerName, @"[^a-zA-Z0-9°¡-ÆR]", "", RegexOptions.Singleline);
        if (_playerName.Length<2 || _playerName.Length>8 || _playerName.Contains(" ") || !_playerName.Equals(check)) 
        {
            string text = "»ç¿ëÇÒ ¼ö ¾ø´Â ´Ð³×ÀÓ ÀÔ´Ï´Ù.";
            _basePopup.OpenUI(); 
            _basePopup.UpdateUI(text,false);
            _basePopup.OnClickConfirmButtonInjection(()=>_basePopup.CloseUI(false));
        }
        else
        {
            string text = $"Á¤¸»·Î \"{_playerName}\"(À¸)·Î ÇÏ½Ã°Ú½À´Ï±î?";
            _basePopup.OpenUI();
            _basePopup.UpdateUI(text);
            _basePopup.OnClickConfirmButtonInjection(InputName);
        }
    }
        
    public void InputName()
    {
        _playerName = _nameInput.text;
        PlayerPrefs.SetString("CurrentPlayerName", _playerName);

        LoadLoadingScene();
    }

    private void LoadLoadingScene()
    {
        LoadingScene.SetNextScene(Define.SceneType.GameScene);
        SceneManager.LoadScene(Define.SceneType.LoadingScene.ToString());
    }
}
