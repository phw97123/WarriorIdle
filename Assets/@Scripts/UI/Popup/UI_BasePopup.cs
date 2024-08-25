using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class UI_BasePopup : UI_Base
{
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private TextMeshProUGUI _description;

    private Action _onClickConfirm; 

    private void Start()
    {
        CloseUI(false);
        _confirmButton.onClick.AddListener(OnClickConfirmButton);
        _cancelButton.onClick.AddListener(()=> CloseUI()); 
    }

    public override void OpenUI()
    {
        base.OpenUI();

        _confirmButton.gameObject.SetActive(true);
        _cancelButton.gameObject.SetActive(true);
    }

    public void UpdateUI(string text, bool isCancelBtn = true)
    {
        _description.text = text;
        if(!isCancelBtn)
            _cancelButton.gameObject.SetActive(false);
    }

    private void OnClickConfirmButton()
    {
        _onClickConfirm?.Invoke(); 
    }

    public void OnClickConfirmButtonInjection(Action action)
    {
        _onClickConfirm = action; 
    }
}
