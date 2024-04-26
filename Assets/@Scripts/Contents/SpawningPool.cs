using System.Collections;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    private float _spawnInterval = 1.0f;
    private int _maxEnemyCount = 10;
    Coroutine _coUpdateSpawningPool;

    public bool Stopped { get; set; } = false;

    private void Start()
    {
        _coUpdateSpawningPool = StartCoroutine(COUpdateSpawningPool()); 
    }

    private IEnumerator COUpdateSpawningPool()
    {
        while (true)
        {
            TrySpawn();
            yield return new WaitForSeconds(_spawnInterval); 
        }
    }

    private void TrySpawn()
    {
        if (Stopped) return;

        int enemyCount = Managers.ObjectManager.Enemys.Count;
        if (enemyCount >= _maxEnemyCount)
            return;

        Vector2 randPos = Utils.GenerateEnemySpawnPosition(Managers.ObjectManager.Player.transform.position, 10, 15);

        // TODO : Stage에 맞게 설정하기
        EnemyController ec = Managers.ObjectManager.Spawn<EnemyController>(randPos,1); 
    }
}

