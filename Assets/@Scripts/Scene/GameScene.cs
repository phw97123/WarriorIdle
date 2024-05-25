using System.Collections;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class GameScene : MonoBehaviour
{
    private SpawningPool _spawningPool;

    private Define.StageType _stageType;

    private BossController _boss;

    private FadeController _fadeController;

    private float _remainingTime;

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
        Managers.GameManager.Init();
        Managers.SoundManager.Init();

        // Player
        var player = Managers.ObjectManager.Spawn<PlayerController>(Vector3.zero);

        // MonsterSpawner
        _spawningPool = gameObject.AddComponent<SpawningPool>();

        // Map 
        Managers.GameManager.SetStageMap();

        // SceneUI
        var uiHud = Managers.UIManager.ShowSceneUI<UI_Hud>();
        uiHud.UI_StageInfo.TryBossButtonInjection(MoveToBossStage);
        uiHud.UI_StageInfo.UpdateUI(Managers.GameManager.StageData);
        uiHud.UI_StageInfo.UpdateStageExp(Managers.GameManager.KillCount);

        Managers.GameManager.OnStageUiUpdate -= uiHud.UI_StageInfo.UpdateUI;
        Managers.GameManager.OnStageUiUpdate += uiHud.UI_StageInfo.UpdateUI;

        Managers.GameManager.OnKillCountChanged -= uiHud.UI_StageInfo.UpdateStageExp;
        Managers.GameManager.OnKillCountChanged += uiHud.UI_StageInfo.UpdateStageExp;

        StageType = StageType.Normal;
        Managers.UIManager.GetSceneUI<UI_Hud>().ActivateStageInfo(StageType);

        // Controller
        GameObject controllerRote = new GameObject() { name = "@Controller" };
        Utils.CreateGameObject<GrowthController>(controllerRote.transform);
        Utils.CreateGameObject<EquipmentController>(controllerRote.transform);
        Utils.CreateGameObject<SummonsController>(controllerRote.transform);
        Utils.CreateGameObject<ShopController>(controllerRote.transform);
        Utils.CreateGameObject<DungeonController>(controllerRote.transform);

        // BGM 
        Managers.SoundManager.Play(STAGE1, Define.AudioType.Bgm);
    }

    #region Stage
    private void MoveToBossStage()
    {
        StageType = Define.StageType.Boss;
        Managers.GameManager.Player.OnPlayerChangedIdleStage();
        Managers.GameManager.Player.PlayerData.SetMax();
        StopAllCoroutines();
        Managers.ObjectManager.DespawnAllEnemy();

        if (_fadeController == null)
            _fadeController = Managers.UIManager.GetSceneUI<UI_Hud>().FadeController;

        _fadeController.RegisterCompletedCallback(BossSpawn);

        _fadeController.RegisterFadeInCallback(() =>
        {
            Managers.UIManager.GetSceneUI<UI_Hud>().ActivateStageInfo(StageType);
        });
        _fadeController.StartFade();
    }

    private void BossSpawn()
    {
        Vector2 spawnPos = Utils.GenerateEnemySpawnPosition(Managers.GameManager.Player.transform.position, 5, 10);
        _boss = Managers.ObjectManager.Spawn<BossController>(spawnPos, Managers.GameManager.StageData.bossID);
        StartCoroutine(BossStage());

        Managers.UIManager.GetSceneUI<UI_Hud>().UI_BossStageInfo.Init(_boss.enemyData);

        _boss.enemyData.characterData.OnBossChangedHp -= Managers.UIManager.GetSceneUI<UI_Hud>().UI_BossStageInfo.UpdateHpUI;
        _boss.enemyData.characterData.OnBossChangedHp += Managers.UIManager.GetSceneUI<UI_Hud>().UI_BossStageInfo.UpdateHpUI;
    }

    private IEnumerator BossStage()
    {
        float startTime = Time.time;
        float totalDuration = 60f;
        _remainingTime = totalDuration;

        while (!_boss.isDead && _remainingTime >= 0 && !Managers.GameManager.Player.isDead)
        {
            float elapsedTime = Time.time - startTime;
            _remainingTime = totalDuration - elapsedTime;
            Managers.UIManager.GetSceneUI<UI_Hud>().UI_BossStageInfo.UpdateTimer(_remainingTime);
            yield return null;
        }

        if (_boss.isDead)
        {
            Managers.GameManager.StageData.isClear = true;
            Managers.GameManager.CurrentStageIndex++;
            Managers.GameManager.KillCount = 0;
        }
        else
        {
            Managers.GameManager.KillCount = Managers.GameManager.StageData.nextStageEnemyCount;
        }

        yield return new WaitForSeconds(3.0f);

        MoveToNormalStage();
    }

    private void MoveToNormalStage()
    {
        Managers.GameManager.Player.PlayerData.SetMax();
        //_fadeController.RegisterCompletedCallback(() =>
        //{
        //});

        _fadeController.RegisterFadeInCallback(() =>
        {
            StageType = Define.StageType.Normal;
            Managers.UIManager.GetSceneUI<UI_Hud>().ActivateStageInfo(StageType);
        });


        _fadeController.StartFade();

        Managers.GameManager.SetStageMap();
    }
    #endregion 

}
