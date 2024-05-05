using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class StageDataSO : ScriptableObject
{
    [SerializeField] private StageData[] stageDatas;

    public StageData GetStageData(int stageNum)
    {
        return stageDatas[stageNum];
    }
}
