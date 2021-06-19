using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : StateMachineBehaviour
{
    //A behaviour that shoots a ranged object at the player


    [SerializeField] GameObject rangedObject;
    [SerializeField] float timeTillObjectSpawns;
    [SerializeField] float speedOfProjectile;

    [Header("Behaviours that can happen if charge is finished ")]
    [SerializeField] AttackMove[] ifFinishedBehaviours;
    bool firstTime =false;
    float timer;
    Vector2 dir;
    EnemyRotator enemyRotator;
    int randomChoiceIfFinished;
    Transform throwLocation;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        enemyRotator = animator.transform.GetChild(0).GetComponent<EnemyRotator>();
        enemyRotator.rotateToPlayer();
        throwLocation = animator.transform.GetChild(0).GetChild(0);
        randomChoiceIfFinished = Random.Range(0, ifFinishedBehaviours.Length);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer > timeTillObjectSpawns)
        {
            if (!firstTime)
            {
                //Spawn Object
                GameObject projectile=Instantiate(rangedObject,throwLocation.position,Quaternion.identity);
                dir = enemyRotator.getDirToPlayer(throwLocation);

                projectile.GetComponent<Projectile>().dir = dir;
                projectile.GetComponent<Projectile>().speed = speedOfProjectile;

                //SET PARAMETERS FOR ANIMATOR   
                if(ifFinishedBehaviours.Length!=0)
                for (int i = 0; i < ifFinishedBehaviours[randomChoiceIfFinished].moveParameters.Length; i++)//Set the animator parameters set in the editor
                {
                    animator.SetBool(ifFinishedBehaviours[randomChoiceIfFinished].moveParameters[i].ParameterName,
                        ifFinishedBehaviours[randomChoiceIfFinished].moveParameters[i].Value);
                }

                firstTime = true;
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        firstTime = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
