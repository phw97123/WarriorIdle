using System.Collections;
using System.Linq;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    private float _spawnInterval = 0.5f;
    private int _maxEnemyCount = 50;
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

        int randEnemy = Random.Range(0, _gameManager.stageData.GetStageData().enemyIDs.Count());

        EnemyController ec = _objectManager.Spawn<EnemyController>(randPos, _gameManager.stageData.GetStageData().enemyIDs[randEnemy]);
    }

    private void OnDestroy()
    {
        if (_coUpdateSpawningPool != null)
        {
            StopCoroutine(_coUpdateSpawningPool);
            _coUpdateSpawningPool = null;
        }
    }
}

