using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AttackMove
{
    //A attackMove that contains Parameters for animator that will be called  
    [Header("Parameters for AttackMove")]
    public AttackMoveParameter[] moveParameters;
}
[System.Serializable]
public struct AttackMoveForChase
{
    //A attackMove that contains Parameters for animator that will be called  
    [Header("Parameters for AttackMove")]
    public AttackMoveParameter[] moveParameters;
    public float speedOfChase;
    public float distanceOfChase;
    public float chaseTime;

}
