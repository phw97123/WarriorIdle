using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    private Action _onFadeInCallback;
    private Action _oncCompletedCallback;

    public void StartFade()
    {
        _panel.SetActive(true);
        StartCoroutine(COFade());
    }

    private IEnumerator COFade()
    {
        float elapsedTime = 0f;
        float fadeTime = 1.5f;

        while (elapsedTime <= fadeTime)
        {
            _panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1f, elapsedTime / fadeTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(1.0f);
        _onFadeInCallback?.Invoke();


        elapsedTime = 0f;
        while (elapsedTime <= fadeTime)
        {
            _panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f, 0f, elapsedTime / fadeTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _panel.SetActive(false);
        _oncCompletedCallback?.Invoke();

        _onFadeInCallback = null;
        _oncCompletedCallback = null;
        yield break;
    }

    public void RegisterCompletedCallback(Action callback)
    {
        _oncCompletedCallback = callback;
    }

    public void RegisterFadeInCallback(Action callback)
    {
        _onFadeInCallback = callback;
    }
}
