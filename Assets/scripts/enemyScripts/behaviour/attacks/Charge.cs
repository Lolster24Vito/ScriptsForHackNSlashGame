using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : StateMachineBehaviour
{
    //A enemy behaviour that  charges the enemy in a certain direction to the player

    [SerializeField] LayerMask attackableLayer;

    [SerializeField] GameObject hitEffect;    //Hit effect that appears when player is hit

    [SerializeField] LayerMask walls;    //LayerMask for walls.So that the the enemy doesn't hold the attack on the wall

    [Header("Charge Variables")]
    [SerializeField] float speed;    //The speed of the charge
    [SerializeField] float distance;    //The distance that the charge will cross unitl stopping
    [SerializeField] float timeTillRestart;    //Time till the charge stops 

    [Header("Collider stuff")]

    [SerializeField] Vector2 chargeColliderSize;    //Collider check size
    [SerializeField] Vector3 OffsetChargeCollider;    //Collider check offset from center

    [Header("Knockback")]
    [SerializeField] bool hasKnockback;
    [SerializeField] float knockbackForce;    //How fast will the player knockback be
    [SerializeField] float knockbackTime;     //how long will the knockback last


   

    [Header("Behaviours that can happen if charge hits player ")]
    [SerializeField] AttackMove[] ifHitPlayerBehaviours;

    [Header("Behaviours that can happen if charge is finished ")]
    [SerializeField] AttackMove[] ifFinishedBehaviours;

    Transform player;
    bool firstTime=false;
    bool hasHitPlayerOnce = false;    //A bool that stops the enemy from damaging the player multiple times
    Vector3 dirToPlayer;
    Rigidbody2D rb;
    Vector2 originalPos;
    Transform transform;
     float timer;
    int randomChoiceIfFinished;
    int randomChoiceIfHit;




    //So this is for performance...Animator.SetBool("String") collects a lot of garbage in the memory
    int idCharge = Animator.StringToHash("Charge");
    int idCharge1 = Animator.StringToHash("Charge1");

    int idChase = Animator.StringToHash("Chase");
    int idIdle = Animator.StringToHash("Idle");
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
     //The first time bool is there as a void start function....OnStateEnter happens everytime a Animation start is played
        if (!firstTime)
        {
            //Decide which behaviour to do if there are multiple
            randomChoiceIfFinished = Random.Range(0, ifFinishedBehaviours.Length);
            randomChoiceIfHit = Random.Range(0, ifHitPlayerBehaviours.Length);


            player = GameObject.FindGameObjectWithTag("Player").transform;
            rb = animator.GetComponent<Rigidbody2D>();
            transform = animator.transform;

            firstTime = true;
            //dirToPlayer insures that during chargeUp the enemy will pick his dir only once and not follow the player endlesly....(This is added because if the dir is already picked during the attack it wouldn't really matter if the player dodged or not)
            dirToPlayer = animator.GetComponent<Enemy>().helperDirToPlayer;
            originalPos = animator.transform.position;
            timer = timeTillRestart;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Go into idle if a certain distance has been travelled,if it detects a wall that it's going into or if a certain time has just passed
        //If those conditions aren't met move in a direction and reduce time
        if (
            (Vector2.Distance(animator.transform.position, originalPos) < distance)&&
            !(Physics2D.OverlapBox(transform.position + OffsetChargeCollider, chargeColliderSize, 0f, walls))&&
            (timer>0)
            )
        {
            timer -= Time.deltaTime;
            rb.MovePosition(animator.transform.position + (dirToPlayer * speed * Time.deltaTime));
        }
        else
        {
            firstTime = false;
            
            //SET PARAMETERS FOR ANIMATOR     
                    for(int i=0;i< ifFinishedBehaviours[randomChoiceIfFinished].moveParameters.Length; i++)//Set the animator parameters set in the editor
                    {
                        animator.SetBool(ifFinishedBehaviours[randomChoiceIfFinished].moveParameters[i].ParameterName,
                            ifFinishedBehaviours[randomChoiceIfFinished].moveParameters[i].Value);
                    }
                
            
     

        }
        if (!hasHitPlayerOnce)
        {
            Collider2D[] collider = Physics2D.OverlapBoxAll(transform.position + OffsetChargeCollider, chargeColliderSize, 0f,attackableLayer);
            foreach (Collider2D col in collider)
            {
                if (col.CompareTag("Player"))
                {
                    //DO DAMAGE TO PLAYER
                    col.GetComponent<Health>().DecreaseHealth(1);
                    col.GetComponent<CharecterAttackController>().isHurt = true;

                   
                    if (hasKnockback)
                    {
                        col.GetComponent<CharecterAttackController>().Hurt((col.transform.position - animator.transform.position), knockbackForce, knockbackTime);
                    }

                    //Spawn a hit effect in the raycast hitPoint between the player and the enemy
                    //Simplified:Spawn Hit effect in the place that it should be
                    RaycastHit2D Ray = Physics2D.Raycast(transform.position, (col.transform.position - transform.position).normalized);
                    Instantiate(hitEffect, new Vector3(Ray.point.x, Ray.point.y, -5), Quaternion.identity,col.transform);
                    //TELL THE SCRIPT TO STOP MOVING
                    hasHitPlayerOnce = true;
                    firstTime = false;


                    //SET PARAMETERS FOR ANIMATOR 
                   
                   for(int i = 0; i < ifHitPlayerBehaviours[randomChoiceIfHit].moveParameters.Length; i++)//Set the animator parameters set in the editor
                   {
                        animator.SetBool(ifHitPlayerBehaviours[randomChoiceIfHit].moveParameters[i].ParameterName,
                              ifHitPlayerBehaviours[randomChoiceIfHit].moveParameters[i].Value);
                    }
                        
                    
                   
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasHitPlayerOnce = false;

    }

  

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + OffsetChargeCollider, chargeColliderSize);
    }
}
