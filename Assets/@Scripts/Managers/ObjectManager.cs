using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class ObjectManager
{
    public PlayerController Player { get; private set; }
    public HashSet<EnemyController> Enemys { get; } = new HashSet<EnemyController>();
    public HashSet<ItemController> Items { get; } = new HashSet<ItemController>();

    public T Spawn<T>(Vector3 position, int templateID = 0) where T : BaseController
    {
        System.Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            GameObject go = Managers.ResourceManager.Instantiate(PLAYER_PREFAB_NAME, pooling: true);
            go.name = "Player";

            PlayerController pc = go.GetOrAddComponent<PlayerController>();
            Player = pc;

            pc.Init();

            return pc as T;
        }
        else if (type == typeof(EnemyController))
        {
            string name = "";

            switch (templateID)
            {
                case GOBLIN_ID:
                    name = "Goblin";
                    break;

                case MUSHROOM_ID:
                    name = "Mushroom";
                    break;
                case SKELETON_ID:
                    name = "Skefleton";
                    break;
                case FLYINGEYE_ID:
                    name = "FlyingEye";
                    break;
            }

            GameObject go = Managers.ResourceManager.Instantiate(name + ".prefab", pooling: true);
            go.transform.position = position;

            EnemyController ec = go.GetOrAddComponent<EnemyController>();
            Enemys.Add(ec);

            ec.Init();

            return ec as T;
        }
        else if (type == typeof(ItemController))
        {
            GameObject go = Managers.ResourceManager.Instantiate(ITEM_PREFAB_NAME, pooling: true);
            go.transform.position = position;

            ItemController ic = go.GetOrAddComponent<ItemController>();
            Items.Add(ic);

            ic.Init();

            return ic as T;
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

        Managers.ResourceManager.Destroy(obj.gameObject);
    }

    public void DespawnAllEnemy()
    {
        var enemys = Enemys.ToList();
        foreach (var enemy in enemys)
            Despawn<EnemyController>(enemy);
    }
}
