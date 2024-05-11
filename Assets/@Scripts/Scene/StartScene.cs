using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    void Start()
    {
        Managers.ResourceManager.LoadAllAsync<Object>("StartScene", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count} / {totalCount}");

            if (count == totalCount)
            {
                StartLoaded();
            }
        }); 
    }

    private void StartLoaded()
    {
        var StartUI = Managers.ResourceManager.Instantiate("UI_StartScreen.prefab");
        StartUI.GetComponent<UI_StartScreen>().StartButtonInjection(LoadScene); 
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("GameScene"); 
    }
}
