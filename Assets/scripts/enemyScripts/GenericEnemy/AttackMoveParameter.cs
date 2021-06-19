using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct AttackMoveParameter
{
    //Contains parameter names and bools that are used in other structs for the animator
    [Header("Name for Move")]
    public string ParameterName;
    [Header("Bool for Move")]
    public bool  Value;
}
