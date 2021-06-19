using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    //A enemy behaviour that just stands still

    int randomChoiceIfFinished;
    float timer;
    [SerializeField] float timeToWaitmin;
    [SerializeField] float timeToWaitMax;

    //Behaviours that can happen if idle is finished

    [Header("Behaviours that can happen if charge is finished ")]
    [SerializeField] AttackMove[] ifFinishedBehaviours;



    bool firstTime =false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!firstTime)
        {
            firstTime = true;
            timer = Random.Range(timeToWaitmin, timeToWaitMax);
            randomChoiceIfFinished = Random.Range(0, ifFinishedBehaviours.Length);

        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If timer has run out do the next attack move
        if (timer <= 0)
        {
            firstTime = false;
            for(int i=0;i< ifFinishedBehaviours[randomChoiceIfFinished].moveParameters.Length; i++)
            {
                animator.SetBool(ifFinishedBehaviours[randomChoiceIfFinished].moveParameters[i].ParameterName
                    , ifFinishedBehaviours[randomChoiceIfFinished].moveParameters[i].Value);
            }


        }
        else
        {
            timer -= Time.deltaTime;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      
    }
   

}
