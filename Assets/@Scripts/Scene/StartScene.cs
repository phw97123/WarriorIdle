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
        Managers.SoundManager.Init();

        var StartUI = Managers.ResourceManager.Instantiate("UI_StartScene.prefab");
        StartUI.GetComponent<UI_StartScene>().StartButtonInjection(LoadNameSettingPopup);

        Managers.SoundManager.Play(Define.STAGE1, Define.AudioType.Bgm);
    }

    public void LoadNameSettingPopup()
    {
        GameObject nameSettingUI = Managers.ResourceManager.Instantiate("UI_NameSettingPopup.prefab");
    }
}
