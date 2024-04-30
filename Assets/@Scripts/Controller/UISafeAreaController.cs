using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISafeAreaController : MonoBehaviour
{
    private Vector2 _minAnchor; 
    private Vector2 _maxAnchor;

    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        _minAnchor = Screen.safeArea.min; 
        _maxAnchor = Screen.safeArea.max;

        _minAnchor.x /= Screen.width;
        _maxAnchor.x /= Screen.width;
        _minAnchor.y /= Screen.height;
        _maxAnchor.y /= Screen.height;

        rectTransform.anchorMin = _minAnchor;
        rectTransform.anchorMax = _maxAnchor;
    }
}
