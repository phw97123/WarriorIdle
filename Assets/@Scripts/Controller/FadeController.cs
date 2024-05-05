using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    private Action onCompleteCallback;

    public void FadeOut()
    {
        _panel.SetActive(true);
        StartCoroutine(COFadeOut());
    }

    public void FadeIn()
    {
        _panel.SetActive(true);
        StartCoroutine(COFadeIn());
    }

    public void FadeInOut()
    { 
        _panel.SetActive(true);
        StartCoroutine(COFadeInOut());
    }

    private IEnumerator COFadeInOut()
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

        elapsedTime = 0f; 
        while (elapsedTime <= fadeTime)
        {
            _panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f, 0f, elapsedTime / fadeTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _panel.SetActive(false);
        onCompleteCallback?.Invoke();
        yield break;
    }

    private IEnumerator COFadeOut()
    {
        float elapsedTime = 0f;
        float fadeTime = 3.0f;

        while (elapsedTime <= fadeTime)
        {
            _panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 1f, elapsedTime / fadeTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _panel.SetActive(false);
        onCompleteCallback?.Invoke();
        yield break;
    }

    private IEnumerator COFadeIn()
    {
        float elapsedTime = 0f;
        float fadeTime = 3.0f;

        while (elapsedTime <= fadeTime)
        {
            _panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f, 0f, elapsedTime / fadeTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _panel.SetActive(false);
        onCompleteCallback?.Invoke();
        yield break;
    }

    public void RegisterCallback(Action callback)
    {
        onCompleteCallback = callback;
    }
}
