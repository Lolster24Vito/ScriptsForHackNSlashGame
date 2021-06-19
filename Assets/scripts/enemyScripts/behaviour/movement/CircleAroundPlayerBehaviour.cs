using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAroundPlayerBehaviour : StateMachineBehaviour
{
    //A enemy behaviour that  rotates the enemy  around the player


    //The random angle that the enemy is going to rotate around the player
    [SerializeField] float angle;
    [SerializeField] [Range(-1, 0.5f)] float moveNearPlayerPercentage = 0.25f;
    [SerializeField] float distanceFromPlayerForNextMove = 0.65f;

    float distance;

    //Posible next attackMoves after the rotation
    [SerializeField] AttackMoveForChase[] attackMoves;

    float cirlceTimer;
    bool firstTime = false;
    Transform player;
    Transform transform;
    Rigidbody2D rb;
    int randomChoice;

    int randDir;
    Vector3 pos;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!firstTime)
        {
            cirlceTimer = 0;
            player = GameObject.FindGameObjectWithTag("Player").transform;
            transform = animator.transform;
            rb = animator.GetComponent<Rigidbody2D>();
            firstTime = true;
            randomChoice = Random.Range(0, attackMoves.Length);
            randDir = Random.Range(0, 2) * 2 - 1;
            angle = Random.Range(-180, 180);

            Debug.Log(animator.gameObject.name + " " + angle);
            distance = Random.Range(0f, 3f);
            pos = getCirclePosition();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {




        //If has cirlce timer has run out  do the nextMove
        if (cirlceTimer > attackMoves[randomChoice].chaseTime)
        {
            for (int i = 0; i < attackMoves[randomChoice].moveParameters.Length; i++)
            {
                animator.SetBool(attackMoves[randomChoice].moveParameters[i].ParameterName, attackMoves[randomChoice].moveParameters[i].Value);
            }

            cirlceTimer = 0;
            firstTime = false;

        }
        else
        {

          
          

            cirlceTimer += Time.deltaTime;

            Vector2 moveToPos = Vector2.MoveTowards(animator.transform.position, pos, attackMoves[randomChoice].speedOfChase * Time.deltaTime);
            rb.MovePosition(moveToPos);

            //If a certain distance is reached do the attack or if the distance is too far do the next attack move
            if (Vector2.Distance(player.position, animator.transform.position) < attackMoves[randomChoice].distanceOfChase ||
                Vector2.Distance(transform.position, pos) < 0.7f 
                )
            {
                for (int i = 0; i < attackMoves[randomChoice].moveParameters.Length; i++)
                {
                    animator.SetBool(attackMoves[randomChoice].moveParameters[i].ParameterName, attackMoves[randomChoice].moveParameters[i].Value);
                }
                animator.GetComponent<Enemy>().getDirToPlayer();
                firstTime = false;
            }
        }



    }


    Vector2 getCirclePosition()
    {
        float t = randDir;
        Vector3 dif = transform.position - player.position+ new Vector3(0.5f,0.5f,0)*distance;
        float x1 = (dif.x * Mathf.Cos(angle * t)) - (dif.y * Mathf.Sin(angle * t));
        float y1 = (dif.y * Mathf.Cos(angle * t)) + (dif.x * Mathf.Sin(angle * t));
         Vector2 finalPos=Vector2.LerpUnclamped(new Vector2(x1 + player.position.x, y1 + player.position.y), player.position, moveNearPlayerPercentage);
        Debug.Log(finalPos);
        return finalPos;
    }
    int GetPosOrNegNumb()
    {
        return Random.Range(0, 2) * 2 - 1;

    }
}
