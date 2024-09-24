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
        var StartUI = Managers.ResourceManager.Instantiate("UI_StartScene.prefab");

        StartUI.GetComponent<UI_StartScene>().StartButtonInjection(() =>
        {
            if (PlayerPrefs.HasKey("CurrentPlayerName"))
            {
                LoadingScene.SetNextScene(Define.SceneType.GameScene);
                SceneManager.LoadScene(Define.SceneType.LoadingScene.ToString());
            }
            else
            {
                LoadNameSettingPopup();
            }
        });
    }

    public void LoadNameSettingPopup()
    {
        Managers.ResourceManager.Instantiate("UI_NameSettingPopup.prefab");
    }
}
