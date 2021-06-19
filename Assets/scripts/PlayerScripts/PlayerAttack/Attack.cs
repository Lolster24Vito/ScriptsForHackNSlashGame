using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{

    [Header("Which animation to use based on it's parameters")]
    public AttackMove attackParametersForAnimator;



    [Header("Attack Variables")]

    [Tooltip("//The wait after a attack is at last frame")]
    [SerializeField] public float attackWait;  //The wait after a attack is at last frame

    [Tooltip("//The maximum distance a attack can travel from it's original position")]
    [SerializeField] public float attackMoveDistance;//Maximum distance of the player attack while moving

    [Tooltip("How fast is the player going to move with the attack")]
    [SerializeField] public float attackMoveSpeed;//At which speed will the attack move happen



    [Header("Collider detection stuff")]

    [SerializeField] public float xColsize; //The x size of the collider that checks if enemy is hit
    [SerializeField] public float yColsize; //The y size of the collider that checks if enemy is hit
    [Header("Knockback")]
    public bool hasKnockback;
    [Tooltip("//How much will the knockback move the enemy")]
    public float knockbackForce;//How much will the knockback move the enemy
    [Tooltip("How long will the knockback force push the enemy")]
    public float knockbackTime;//How long will the knockbackForce knockback the enemy

    [HideInInspector] public bool wasLastFrameWaitDone = false;










}
