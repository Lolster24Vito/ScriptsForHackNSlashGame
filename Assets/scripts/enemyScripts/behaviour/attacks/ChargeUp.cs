using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeUp : StateMachineBehaviour
{
    //A reducted behaviour that shouldn't be used anymore that simply changes animator parameters
    [SerializeField] string nextParameter;
    [SerializeField] string currentParameter;
[SerializeField] bool canBeHurtDuringChargeUp;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state


    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("shiieet");

    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (canBeHurtDuringChargeUp)
        {
            if (animator.GetBool(Animator.StringToHash("Hurt")))
            {
                animator.SetBool(currentParameter, false);
                animator.SetBool(nextParameter, false);
            }
        }
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //  Debug.Log("FUUUKKKKKKKKKKKKK");
        if (canBeHurtDuringChargeUp)
        {
            if (animator.GetBool("Hurt"))
            {
                animator.SetBool(currentParameter, false);
                animator.SetBool(nextParameter, false);
            }
            else
            {
                animator.SetBool(currentParameter, false);
                animator.SetBool(nextParameter, true);
            }

            }
        if (!canBeHurtDuringChargeUp)
        {
            animator.SetBool(currentParameter, false);
            animator.SetBool(nextParameter, true);
        }
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
