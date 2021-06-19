using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class AttackStatusEffects : ScriptableObject
{
    public string name;
    public float durationOfEffectInSeconds;
    public int damageOfEffect;
    public float dealDamageEveryAmountOfSeconds;
    public GameObject effectWhileActive;
    public abstract void Initialize(GameObject enemy);
    public abstract IEnumerator DealEffect(GameObject enemy);
}
