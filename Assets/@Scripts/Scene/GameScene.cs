using System.Collections;
using UnityEngine;
using static Define;

public class GameScene : MonoBehaviour
{
    private SpawningPool _spawningPool;

    private Define.StageType _stageType;

    private BossController _boss;

    private FadeController _fadeController;
    private RewardController _rewardController;

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
                    case Define.StageType.Dungeon:
                        _spawningPool.Stopped = true;
                        break;
                }

            }
        }
    }

    private void Start()
    {
        StartLoaded();
    }

    public void StartLoaded()
    {
        var player = Managers.ObjectManager.Spawn<PlayerController>(Vector3.zero);
        Managers.DataManager.Init();
        Managers.SoundManager.Init();

        // MonsterSpawner
        _spawningPool = gameObject.GetComponent<SpawningPool>();

        // Map 
        Managers.GameManager.SetStageMap();

        // SceneUI
        var uiHud = Managers.UIManager.ShowSceneUI<UI_Hud>();
        uiHud.UI_StageInfo.TryBossButtonInjection(MoveToBossStage);
        uiHud.UI_StageInfo.UpdateUI(Managers.DataManager.stageData.GetStageData());
        uiHud.UI_StageInfo.UpdateStageExp(Managers.DataManager.stageData.KillCount);

        Managers.DataManager.stageData.OnStageUiUpdate -= uiHud.UI_StageInfo.UpdateUI;
        Managers.DataManager.stageData.OnStageUiUpdate += uiHud.UI_StageInfo.UpdateUI;

        Managers.DataManager.stageData.OnKillCountChanged -= uiHud.UI_StageInfo.UpdateStageExp;
        Managers.DataManager.stageData.OnKillCountChanged += uiHud.UI_StageInfo.UpdateStageExp;

        StageType = StageType.Normal;
        Managers.UIManager.GetSceneUI<UI_Hud>().ActivateStageInfo(StageType);

        Managers.GameManager.onStartDungeon -= StartDungeon;
        Managers.GameManager.onStartDungeon += StartDungeon;

        // Controller
        GameObject controllerRote = new GameObject() { name = "@Controller" };
        Utils.CreateGameObject<GrowthController>(controllerRote.transform);
        Utils.CreateGameObject<EquipmentController>(controllerRote.transform);
        Utils.CreateGameObject<SummonsController>(controllerRote.transform);
        Utils.CreateGameObject<ShopController>(controllerRote.transform);
        Utils.CreateGameObject<DungeonController>(controllerRote.transform);
        Utils.CreateGameObject<SkillController>(controllerRote.transform);
        Utils.CreateGameObject<AutoSaveDataController>(controllerRote.transform);

        _rewardController = Utils.CreateGameObject<RewardController>(controllerRote.transform).GetComponent<RewardController>();

        _fadeController = Managers.UIManager.GetSceneUI<UI_Hud>().FadeController;

        // BGM 
        Managers.SoundManager.Play(STAGE1, Define.AudioType.Bgm);
    }

    #region Stage
    private void MoveToBossStage()
    {
        Managers.UIManager.GetSceneUI<UI_Hud>().bg.SetActive(true); 
        StageType = Define.StageType.Boss;
        Managers.GameManager.Player.OnPlayerChangedIdleStage();
        Managers.GameManager.Player.PlayerData.SetMax();
        StopAllCoroutines();
        Managers.ObjectManager.DespawnAllEnemy();

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
        _boss = Managers.ObjectManager.Spawn<BossController>(spawnPos, Managers.DataManager.stageData.GetStageData().bossID);
        Managers.UIManager.GetSceneUI<UI_Hud>().UI_BossStageInfo.SetData(_boss.enemyData);

        StartCoroutine(BossStage());

        _boss.enemyData.characterData.OnBossChangedHp -= Managers.UIManager.GetSceneUI<UI_Hud>().UI_BossStageInfo.UpdateHpUI;
        _boss.enemyData.characterData.OnBossChangedHp += Managers.UIManager.GetSceneUI<UI_Hud>().UI_BossStageInfo.UpdateHpUI;
    }

    private IEnumerator BossStage()
    {
        yield return StartCoroutine(UpdateTimer(60, () => _boss.isDead || Managers.GameManager.Player.isDead, (remainingTime) =>
        {
            Managers.UIManager.GetSceneUI<UI_Hud>().UI_BossStageInfo.UpdateTimer(remainingTime);
        }));

        if (_boss.isDead)
        {
            _rewardController.GetPopup().OpenUI();
            Managers.DataManager.stageData.StageIndex++;
            Managers.DataManager.stageData.KillCount = 0;
        }
        else
        {
            Managers.DataManager.stageData.KillCount = Managers.DataManager.stageData.GetStageData().nextStageEnemyCount;
        }

        yield return new WaitForSeconds(3.0f);
        _rewardController.GetPopup().CloseUI(false);

        MoveToNormalStage();
    }

    private void MoveToNormalStage()
    {
        Managers.GameManager.Player.PlayerData.SetMax();

        _fadeController.RegisterFadeInCallback(() =>
        {
            StageType = Define.StageType.Normal;
            Managers.UIManager.GetSceneUI<UI_Hud>().ActivateStageInfo(StageType);
            Managers.UIManager.GetSceneUI<UI_Hud>().bg.SetActive(false);
        });

        _fadeController.StartFade();

        Managers.GameManager.SetStageMap();
    }

    private void StartDungeon(DungeonDataSO data)
    {
        Managers.UIManager.GetSceneUI<UI_Hud>().bg.SetActive(true);

        Managers.GameManager.Player.PlayerData.SetMax();
        StageType = StageType.Dungeon;
        Managers.ObjectManager.DespawnAllEnemy();
        Managers.UIManager.GetSceneUI<UI_Hud>().UI_BottomBar.CloseDungeonPanel();

        _fadeController.RegisterFadeInCallback(() =>
        {
            Managers.UIManager.GetSceneUI<UI_Hud>().ActivateStageInfo(StageType);
            Managers.UIManager.GetSceneUI<UI_Hud>().UI_DungeonStageInfo.SetData(data);
            DungeonObjectSpawn(data);
        });

        _fadeController.StartFade();

        Managers.GameManager.Player.transform.position = new Vector3(1.5f, 0, 0);
    }

    private void DungeonObjectSpawn(DungeonDataSO data)
    {
        Vector2 spawnPos = Vector2.zero;
        GameObject go = Managers.ObjectManager.Spawn<DungeonObjectController>(spawnPos, (int)data.currencyKeyType).gameObject;
        var doc = go.GetComponent<DungeonObjectController>();

        StartCoroutine(DungeonStage(doc));

        doc.OnTakeDamage -= Managers.UIManager.GetSceneUI<UI_Hud>().UI_DungeonStageInfo.UpdateDamageText;
        doc.OnTakeDamage += Managers.UIManager.GetSceneUI<UI_Hud>().UI_DungeonStageInfo.UpdateDamageText;
    }

    private IEnumerator DungeonStage(DungeonObjectController doc)
    {
        _remainingTime = 30;
        yield return StartCoroutine(UpdateTimer(_remainingTime, () => _remainingTime <= 0f, (remainingTime) =>
        {
            _remainingTime = remainingTime;
            Managers.UIManager.GetSceneUI<UI_Hud>().UI_DungeonStageInfo.UpdateTimer(remainingTime, 30);
        }));

        doc.OnDead();

        _rewardController.GetPopup().OpenUI();
        yield return new WaitForSeconds(3.0f);
        _rewardController.GetPopup().CloseUI(false);
        MoveToNormalStage();
    }

    private IEnumerator UpdateTimer(float duration, System.Func<bool> stopCondition, System.Action<float> onUpdate)
    {
        float startTime = Time.time;
        float remainingTime = duration;

        while (remainingTime > 0 && !stopCondition())
        {
            float elapsedTime = Time.time - startTime;
            remainingTime = duration - elapsedTime; ;
            onUpdate(remainingTime);
            yield return null;
        }
    }
    #endregion

    private void OnApplicationQuit()
    {
        Managers.DataManager.SaveAllData(); 
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Managers.DataManager.SaveAllData();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            Managers.DataManager.SaveAllData();
        }
    }
}
