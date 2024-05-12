using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ProbabilityData
{
    public List<float> probabilityArray;
}

[CreateAssetMenu(menuName = "SummonsDataSO")]
public class SummonsDataSO : ScriptableObject
{
    [SerializeField] List<ProbabilityData> probabilities;
    public int id;
    public Define.SummonsType type;
    public Sprite icon;
    public string slotName;
    public int maxLevel;

    public List<float> Getprobalility(int level)
    {
        return probabilities[level - 1].probabilityArray;
    }
}
