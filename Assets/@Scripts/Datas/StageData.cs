using System.Collections.Generic;
using UnityEditor.AddressableAssets.HostingServices;
using UnityEngine;

[System.Serializable]
public class StageData
{
    public int nextStageEnemyCount;
    public string stageName;
    public string mapName;
    public int bossID;
    public int[] enemyIDs;
    public bool isClear = false;
}
