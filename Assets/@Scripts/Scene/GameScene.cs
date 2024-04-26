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
        var player = Managers.ObjectManager.Spawn<PlayerController>(Vector3.zero); 

        _spawningPool = gameObject.AddComponent<SpawningPool>();

        var map = Managers.ResourceManager.Instantiate("Map.prefab");
        map.name = "@Map"; 
    }
}
