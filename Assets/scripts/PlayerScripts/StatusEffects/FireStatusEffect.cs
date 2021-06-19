using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Status Effeccts/Fire status effects")]

public class FireStatusEffect : AttackStatusEffects
{

    public override void Initialize(GameObject enemy)
    {
        
    }
    public override IEnumerator DealEffect(GameObject enemy)
    {
        float timer = 0f;
        Health enemyHP = enemy.GetComponent<Health>(); 
        if (enemy.GetComponent<Health>() == null) {
            Debug.Log("THe enemy has no health script so the effect cannot activate");
           yield return null;
                }

        while (timer < durationOfEffectInSeconds)
        {
            Debug.Log("dealth" + damageOfEffect + " Damage");
            enemy.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(dealDamageEveryAmountOfSeconds);
            enemy.GetComponent<SpriteRenderer>().color = Color.blue;

            enemyHP.DecreaseHealth(damageOfEffect);
            timer += dealDamageEveryAmountOfSeconds;
        }
    }
}
