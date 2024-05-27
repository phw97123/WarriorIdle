using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Utils 
{
    public static Vector2 GenerateEnemySpawnPosition(Vector2 characterPosition, float minDistance = 10.0f, float maxDistance = 20.0f)
    {
       float angle = Random.Range(0,360) * Mathf.Deg2Rad;
        float distance = Random.Range(minDistance,maxDistance);

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
}
