using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : StateMachineBehaviour
{
    //Chase player behaviour script

    float chaseTimer;

    Vector2 velocityHelper = Vector2.zero;

    //Possible attackMoves for when close to player
    [SerializeField] AttackMoveForChase[] attackMoves;

    bool firstTime = false;
    Transform player;

    Rigidbody2D rb;

    int randomChoice;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


        //The first time bool is there as a void start function....OnStateEnter happens everytime a Animation start is played,
        //so by doing a firstTime bool check I avoid a bug in looping animations
        if (!firstTime)
        {
            chaseTimer = 0;
            player = GameObject.FindGameObjectWithTag("Player").transform;
            rb = animator.GetComponent<Rigidbody2D>();
            firstTime = true;
           // nextMove = (Random.value > 0.5f);
            randomChoice = Random.Range(0, attackMoves.Length);

        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        /*if (velocityHelper != rb.velocity.normalized)
        {
            velocityHelper = rb.velocity.normalized;

        }
        */



        //If chase timer runs out do the attack nontheless
        if (chaseTimer > attackMoves[randomChoice].chaseTime)
        {
           for(int i = 0; i < attackMoves[randomChoice].moveParameters.Length; i++)
            {
                animator.SetBool(attackMoves[randomChoice].moveParameters[i].ParameterName, attackMoves[randomChoice].moveParameters[i].Value);
            }

            chaseTimer = 0;
            firstTime = false;

        }
        //If chase timer hasn't run out
        else
        {
            chaseTimer += Time.deltaTime;

            
            Vector2 moveToPos= Vector2.MoveTowards(animator.transform.position, player.position, attackMoves[randomChoice].speedOfChase * Time.deltaTime);
            rb.MovePosition(moveToPos);

            //If a certain distance is reached do the attack
                if (Vector2.Distance(player.position, animator.transform.position) < attackMoves[randomChoice].distanceOfChase)
                {
                //Set the animator parameters  that were set in the editor for the next attack move
                for (int i = 0; i < attackMoves[randomChoice].moveParameters.Length; i++)
                {
                    animator.SetBool(Animator.StringToHash(attackMoves[randomChoice].moveParameters[i].ParameterName),
                        attackMoves[randomChoice].moveParameters[i].Value);
                }
                animator.GetComponent<Enemy>().getDirToPlayer();
                firstTime = false;
          

            }
        }



    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}
