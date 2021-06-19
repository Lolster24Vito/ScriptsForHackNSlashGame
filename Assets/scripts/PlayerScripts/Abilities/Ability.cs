using UnityEngine;
using System.Collections;

public abstract class Ability : ScriptableObject
{

    public string aName = "New Ability";
    public float castTime;
    public AttackMoveParameter[] abilityAnimatorParametersForCast;
    public AttackMoveParameter[] abilityAnimatorParametersForOver;
    public AttackStatusEffects attackStatusEffect;

    public abstract void Initialize(GameObject obj);

    public abstract void Begin(GameObject obj);

    public abstract void AbilityFunction();

    public abstract void End(GameObject obj);


    public virtual IEnumerator TriggerAbility(GameObject gameObject)
    {
        Begin(gameObject);
        yield return new WaitForSeconds(castTime);
   
        AbilityFunction();
        End(gameObject);

    }




}