using UnityEngine;

[CreateAssetMenu()]
public class StageDataSO : ScriptableObject
{
    public int nextStageEnemyCount;
    public string stageName;
    public string mapName;
    public int bossID;
    public int[] enemyIDs;
}
