using UnityEngine;

public class GameScene : MonoBehaviour
{
    private SpawningPool _spawningPool; 

    private void Start()
    {
        Managers.ResourceManager.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
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
        // Player
        var player = Managers.ObjectManager.Spawn<PlayerController>(Vector3.zero); 

        // MonsterSpawner
        _spawningPool = gameObject.AddComponent<SpawningPool>();

        // Map 
        var map = Managers.ResourceManager.Instantiate("Map.prefab");
        map.name = "@Map";

        // SceneUI
        Managers.UIManager.ShowSceneUI<UI_Hud>(); 
    }
}
