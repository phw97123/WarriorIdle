using System.Collections;
using System.Linq;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    private float _spawnInterval = 1.0f;
    private int _maxEnemyCount = 10;
    private Coroutine _coUpdateSpawningPool;

    private GameManager _gameManager;
    private ObjectManager _objectManager; 

    public bool Stopped { get; set; } = false;

    private void Start()
    {
        _gameManager = Managers.GameManager; 
        _objectManager = Managers.ObjectManager;
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

        int enemyCount = _objectManager.Enemys.Count;
        if (enemyCount >= _maxEnemyCount)
            return;

        Vector2 randPos = Utils.GenerateEnemySpawnPosition(_objectManager.Player.transform.position, 10, 15);

        int randEnemy = Random.Range(_gameManager.StageData.enemyIDs[0], _gameManager.StageData.enemyIDs[_gameManager.StageData.enemyIDs.Count()-1]);
        EnemyController ec =_objectManager.Spawn<EnemyController>(randPos, randEnemy);
    }
}

