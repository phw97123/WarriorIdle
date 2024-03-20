using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums 
{
    public enum Tag
    {
        Player,
        Enemy,
    }

    public enum CharacterState
    {
        Idle,
        Run,
        Attack,
        TakeHit,
        Death
    }
}
