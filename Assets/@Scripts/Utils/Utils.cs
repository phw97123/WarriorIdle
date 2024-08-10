using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static Vector2 GenerateEnemySpawnPosition(Vector2 characterPosition, float minDistance = 10.0f, float maxDistance = 20.0f)
    {
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        float distance = Random.Range(minDistance, maxDistance);

        float xDist = Mathf.Cos(angle) * distance;
        float yDist = Mathf.Sin(angle) * distance;

        Vector2 spawnPosition = characterPosition + new Vector2(xDist, yDist);

        return spawnPosition;
    }

    public static GameObject CreateGameObject<T>(Transform parent = null) where T : BaseController
    {
        GameObject go = new GameObject() { name = typeof(T).Name };
        go.AddComponent<T>();
        go.transform.SetParent(parent);

        return go;
    }

    public static EnemyController FindNearestEnemy()
    {
        EnemyController nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var enemyObject in Managers.ObjectManager.Enemys)
        {
            if (enemyObject.isDead) continue;

            EnemyController enemy = enemyObject;
            float distance = Vector2.Distance(Managers.GameManager.Player.transform.position, enemy.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    public static List<EnemyController> FindEnemiesInRadius(float radius)
    {
        List<EnemyController> enemiesInRange = new List<EnemyController>();

        foreach (var enemyObject in Managers.ObjectManager.Enemys)
        {
            if (enemyObject.isDead) continue;

            EnemyController enemy = enemyObject;
            float distance = Vector2.Distance(Managers.GameManager.Player.transform.position, enemy.transform.position);

            if (distance <= radius)
            {
                enemiesInRange.Add(enemy);
            }
        }

        if (enemiesInRange.Count <= 0)
        {
            Debug.Log("¾øÀ½");
        }
        return enemiesInRange;
    }
}
