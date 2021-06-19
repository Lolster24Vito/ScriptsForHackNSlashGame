using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurt : StateMachineBehaviour
{



    [SerializeField] float timeTilNextMove;


    [Header("Behaviours that can happen after Hurt is finished ")]
    [SerializeField] AttackMove[] afterHurt;
    Rigidbody2D rb;
    int randomChoice;
    float timer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        timer = timeTilNextMove;
        randomChoice= Random.Range(0, afterHurt.Length);
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
            timer = timeTilNextMove;

            for(int j = 0; j < afterHurt.Length; j++)
            {
                if (randomChoice == j)
                {
                    for (int i = 0; i < afterHurt[j].moveParameters.Length; i++)
                    {
                        animator.SetBool(Animator.StringToHash(afterHurt[j].moveParameters[i].ParameterName),
                            afterHurt[j].moveParameters[i].Value);
                    }
                }
            }

        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("IDK");
    }


}
