using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class LoadingScene : MonoBehaviour
{
    private static SceneType _nextSceneType = SceneType.Unknown;
    private UI_LoadingScene _loadingSceneUI;

    private float _updateBarInterval = 0.5f; 

    public static void SetNextScene(SceneType nextSceneType)
    {
        _nextSceneType = nextSceneType;
    }

    private void Start()
    {
        Managers.UIManager.TryGetUIComponent(out _loadingSceneUI);

        Managers.ResourceManager.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            if (count == totalCount)
            {
                LoadNextScene();
            }
        });
    }

    private void LoadNextScene()
    {
        if (_nextSceneType != SceneType.Unknown)
        {
            StartCoroutine(LoadSceneAsync(_nextSceneType));
        }
    }

    private IEnumerator LoadSceneAsync(SceneType nextSceneType)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneType.ToString());
        op.allowSceneActivation = false;

        float simulatedProgress = 0f; 
        while(simulatedProgress < 1f)
        {
            simulatedProgress += Time.deltaTime * _updateBarInterval; 
            _loadingSceneUI.UpdateLoadingBar((int)(simulatedProgress * 100), 100);
            yield return null;
        }

        op.allowSceneActivation = true; 
        while (!op.isDone)
        {
            yield return null;
        }
    }
}
