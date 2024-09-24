using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Define;

public class ObjectManager
{
    public PlayerController Player { get; private set; }

    private EnemyTypeDataSO _enemyTypeDataSO;

    public HashSet<EnemyController> Enemys { get; } = new HashSet<EnemyController>();
    public HashSet<ItemController> Items { get; } = new HashSet<ItemController>();
    public BossController StageBoss { get; private set; }

    public T Spawn<T>(Vector3 position, int templateID = 0) where T : BaseController
    {
        System.Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            GameObject go = Managers.ResourceManager.Instantiate(PLAYER_PREFAB, pooling: true);
            go.name = "Player";

            PlayerController pc = go.GetOrAddComponent<PlayerController>();
            Player = pc;

            pc.Init();

            return pc as T;
        }
        else if (type == typeof(EnemyController))
        {
            string name = GetEnemyName(templateID);

            GameObject go = Managers.ResourceManager.Instantiate(name + ".prefab", pooling: true);
            go.transform.position = position;

            EnemyController ec = go.GetOrAddComponent<EnemyController>();
            Enemys.Add(ec);

            ec.Init();

            return ec as T;
        }
        else if (type == typeof(ItemController))
        {
            GameObject go = Managers.ResourceManager.Instantiate(ITEM_PREFAB, pooling: true);
            go.transform.position = position;

            ItemController ic = go.GetOrAddComponent<ItemController>();
            Items.Add(ic);

            ic.Init();

            return ic as T;
        }
        else if (type == typeof(DamageTextController))
        {
            GameObject go = Managers.ResourceManager.Instantiate(DAMAGETEXT_PREFAB, pooling: true);
            go.transform.position = position;

            DamageTextController dtc = go.GetOrAddComponent<DamageTextController>();

            dtc.Init();

            return dtc as T;
        }
        else if (type == typeof(BossController))
        {
            string name = GetEnemyName(templateID);
            GameObject go = Managers.ResourceManager.Instantiate(name + ".prefab");
            go.transform.position = position;

            BossController bc = go.GetOrAddComponent<BossController>();
            StageBoss = bc; 
            Enemys.Add(bc); 

            bc.Init();
            return bc as T;
        }
        else if (type == typeof(DungeonObjectController))
        {
            string name = "DungeonObject";
            GameObject go = Managers.ResourceManager.Instantiate(name + ".prefab", pooling : true);

            DungeonObjectController doc = go.GetOrAddComponent<DungeonObjectController>();

            Enemys.Add(doc);

            doc.Init(); 
             doc.SetData((CurrencyType)templateID);
            return doc as T; 
        }

        return null;
    }

    public void Despawn<T>(T obj) where T : BaseController
    {
        if (obj.IsValid())
            return;

        System.Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            Player = null;
        }
        else if (type == typeof(EnemyController))
        {
            Enemys.Remove(obj as EnemyController);
        }
        else if (type == typeof(ItemController))
        {
            Items.Remove(obj as ItemController);
        }
        else if (type == typeof(BossController))
        {
            Enemys.Remove(obj as BossController);
            StageBoss = null; 
        }
        else if (type == typeof(DungeonObjectController))
        {
            Enemys.Remove(obj as BossController);
        }

        Managers.ResourceManager.Destroy(obj.gameObject);
    }

    public void DespawnAllEnemy()
    {
        var enemys = Enemys.ToList();
        foreach (var enemy in enemys)
        {
            if (enemy.isDead) continue; 
            Despawn<EnemyController>(enemy);
        }
    }

    public void Destroy()
    {
        DespawnAllEnemy();
        Enemys.Clear(); 
        Items.Clear();
    }

    public string GetEnemyName(int templateID)
    {
        if (_enemyTypeDataSO == null)
            _enemyTypeDataSO = Managers.ResourceManager.Load<EnemyTypeDataSO>("EnemyTypeDataSO.asset");

        EnemyTypeData data = _enemyTypeDataSO.EnemyTypes.Find(enemy => enemy.id == templateID);
        return data.name;
    }
}
