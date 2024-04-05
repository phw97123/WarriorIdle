using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows.Speech;

[System.Serializable]
public class CharacterData
{
    private float speed = 15.0f;
    private int hp = 100;

    public float Speed { get { return speed; } protected set { speed = value; } }
    public int HP { get { return hp; } protected set { hp = value; } }
}
