using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    private SpawningPool _spawningPool;

    private Define.StageType _stageType;

    private EnemyController _boss;

    private FadeController _fadeController;

    public Define.StageType StageType
    {
        get { return _stageType; }
        set
        {
            _stageType = value;
            if (_spawningPool != null)
            {
                switch (value)
                {
                    case Define.StageType.Normal:
                        _spawningPool.Stopped = false;
                        break;
                    case Define.StageType.Boss:
                        _spawningPool.Stopped = true;
                        break;
                }
            }
        }
    }

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
        var map = Managers.ResourceManager.Instantiate("Stage_1_Map.prefab");
        map.name = "@Map";

        // SceneUI
        Managers.UIManager.ShowSceneUI<UI_Hud>();

        Managers.GameManager.Init();

        Managers.GameManager.OnKillCountChanged -= HandleOnKillCountChanged;
        Managers.GameManager.OnKillCountChanged += HandleOnKillCountChanged;

    }

    public void HandleOnKillCountChanged(int killCount)
    {
        if (killCount == Managers.GameManager.StageData.nextStageEnemyCount)
        {
            Managers.GameManager.Player.PlayerData.SetMax();
            StageType = Define.StageType.Boss;
            Managers.ObjectManager.DespawnAllEnemy();

            if (_fadeController == null)
                _fadeController = Managers.UIManager.GetSceneUI<UI_Hud>().fadeController;

            _fadeController.RegisterCallback(BossSpawn);
            _fadeController.FadeInOut();
        }
    }

    private void BossSpawn()
    {
        Vector2 spawnPos = Utils.GenerateEnemySpawnPosition(Managers.GameManager.Player.transform.position, 5, 10);
        _boss = Managers.ObjectManager.Spawn<EnemyController>(spawnPos, Managers.GameManager.StageData.bossID);
        StartCoroutine(BossStage());
    }

    private IEnumerator BossStage()
    {
        yield return new WaitUntil(() => _boss.isDead || TimerExpired());
        yield return new WaitForSeconds(3.0f); 

        MoveToStage(); 
    }

    private bool TimerExpired()
    {
        return Time.timeSinceLevelLoad >= 60f;
    }

    private void MoveToStage()
    {
        Managers.GameManager.KillCount = 0; 
        _fadeController.RegisterCallback(() => StageType = Define.StageType.Normal);
        _fadeController.FadeInOut();
        //if (_boss.isDead)
        //    Managers.GameManager.CurrentStageIndex++;
    }
}
