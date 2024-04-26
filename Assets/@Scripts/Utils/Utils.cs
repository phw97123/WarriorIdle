using System.Collections;
using System.Collections.Generic;
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
}
