using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _isPressed = false;
    private Button _button;

    private WaitForSeconds pushTime = new WaitForSeconds(1f);
    private WaitForSeconds repeatTime = new WaitForSeconds(0.3f);

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
        StartCoroutine(COLongPress()); 
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false; 
    }

    public IEnumerator COLongPress()
    {
        yield return pushTime;
        while (_isPressed)
        {
            Excute();
            yield return repeatTime; 
        }
    }

    private void Excute()
    {
        if (_button != null)
            _button.onClick.Invoke(); 
    }
}
