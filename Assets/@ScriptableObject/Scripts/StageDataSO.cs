using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class StageDataSO : ScriptableObject
{
    public StageData[] stageDatas;

    public StageData GetStageData(int stageIndex)
    {
        return stageDatas[stageIndex];
    }
}
