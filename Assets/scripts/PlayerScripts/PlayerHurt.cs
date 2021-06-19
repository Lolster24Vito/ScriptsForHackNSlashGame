using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : StateMachineBehaviour
{
   
    int idHurt = Animator.StringToHash("isHurt");

    [SerializeField] float timeTillIdle;
    float timer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = timeTillIdle;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

        }
        else
        {
            timer = timeTillIdle;
            animator.SetBool(idHurt, false);
            animator.GetComponent<CharecterAttackController>().isHurt = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("I AM IN PAIN ");
    }
}
