using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : StateMachineBehaviour
{
    //A behaviour script that attacks from a place


    [SerializeField] LayerMask attackableLayer;
    [SerializeField] AudioEvent swingAudio;
    [SerializeField] AudioEvent stabAudio;

    [SerializeField] GameObject hitEffect;    //Hit effect that appears when player is hit
    [SerializeField] GameObject effectBeforeAttack;
    [SerializeField] float timeTillEffectBeforeAttackSpawns;

    [Header("Timing for next attack")]
    [SerializeField] float timeTillNextPhase;    //Time till next AttackMove/Behaviour
    [Header("Timing for attack Checks")]
    [SerializeField] float timeTillAttackChecks;    //Time till the actual attack stars checking for hit
    [SerializeField] float timeTillAttackChecksStops;    //Time till the attack stops checking for hit
    [Header("Attack movement properties")]
    [SerializeField] float timeTillAttackMoves;         //When should the enemy start  moving with his attack
    [SerializeField] float timeTillAttackStopsMoving;   //When should the enemy stop moving with his attack
    [SerializeField] float attackMovementSpeed;   //How fast should the attack be


    [Header("Knockback")]
    [SerializeField] bool hasKnockback;
    [SerializeField] float knockbackForce;    //How powerfull will the attack knockback be
    [SerializeField] float knockbackTime;    //how long will the knockback last


    [Header("ColliderSettings ")]
    [SerializeField] Vector2 attackColliderSize;    //Collider check size
    [SerializeField] Vector3 OffsetChargeCollider;    //Collider check offset from center


    [Header("Behaviour Parameters at start of attack")]
    [SerializeField] AttackMove[] behaviourOnStart;

    [Header("Behaviour Parameters at end of attack")]
    [SerializeField] AttackMove[] behaviourOnEnd;


    [Header("Behaviour Parameters if player is hit  by attack")]
    [SerializeField] AttackMove[] behaviourOnPlHit;

    int randomChoiceForStart;
    int randomChoiceForEnd;
    int randomChoiceForHit;

    float timer;
    Vector3 dirToPlayer;

    bool hasHitPlayerOnce = false;
    private bool hasPlayedswingAudio=false;


    Rigidbody2D rigidbody;
 Transform colliderTr;

    EnemyRotator enemyRotator;

    GameObject helper;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Bool setUp
        enemyRotator = animator.transform.GetChild(0).GetComponent<EnemyRotator>();
        colliderTr = animator.transform.GetChild(0).GetChild(0).transform;

        hasHitPlayerOnce = false;
        timer =0;
        randomChoiceForStart = Random.Range(0, behaviourOnStart.Length);
        randomChoiceForEnd = Random.Range(0, behaviourOnEnd.Length);
        randomChoiceForHit = Random.Range(0, behaviourOnPlHit.Length);


        enemyRotator.rotateToPlayer();

       Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidbody = animator.GetComponent<Rigidbody2D>();
       dirToPlayer = (player.position - animator.transform.position).normalized;

        if (behaviourOnStart.Length != 0)
        {
            for (int i = 0; i < behaviourOnStart[randomChoiceForStart].moveParameters.Length; i++)
            {
                animator.SetBool(behaviourOnStart[randomChoiceForStart].moveParameters[i].ParameterName, behaviourOnStart[randomChoiceForStart].moveParameters[i].Value);
                Debug.Log("Here on the start Right now " + i);

            }
        }

        if (Vector2.Distance(animator.transform.position, player.position)>1.5f)
        {
            if (behaviourOnEnd.Length != 0)
                for (int i = 0; i < behaviourOnEnd[randomChoiceForEnd].moveParameters.Length; i++)
                {
                    animator.SetBool(behaviourOnEnd[randomChoiceForEnd].moveParameters[i].ParameterName, behaviourOnEnd[randomChoiceForEnd].moveParameters[i].Value);
                    //   Debug.Log("Here on the end Right now "+i);

                }
        }
        // animator.SetBool("Attack1", false);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If time till next move isn't over increase timer
        if (timer < timeTillNextPhase)
        {
            timer += Time.deltaTime;
        }
        //If time till next move  is here then set parameters for next attack
        else
        {

            if(behaviourOnEnd.Length!=0)
            for (int i = 0; i < behaviourOnEnd[randomChoiceForEnd].moveParameters.Length; i++)
            {
                animator.SetBool(behaviourOnEnd[randomChoiceForEnd].moveParameters[i].ParameterName,behaviourOnEnd[randomChoiceForEnd].moveParameters[i].Value);
             //   Debug.Log("Here on the end Right now "+i);

            }
            //animator.SetBool("Attack1", true);
        }

        //If hasn't hit player once,and is in "time for attack checks" and hasn't passed the "time till attack checks stops"
        if (!hasHitPlayerOnce&&timer>timeTillAttackChecks&&timer<timeTillAttackChecksStops)
        {
            //Play swing audio
            if (!hasPlayedswingAudio&&swingAudio!=null)
            {
                swingAudio.Play(animator.GetComponent<AudioSource>());
                hasPlayedswingAudio = true;
            }
           // animator.GetComponent<SpriteRenderer>().color = Color.blue;


            //Check if player is hit
            Collider2D[] collider = Physics2D.OverlapBoxAll(colliderTr.position, attackColliderSize, colliderTr.rotation.eulerAngles.z);
            foreach (Collider2D col in collider)
            {
                if (col.CompareTag("Player"))
                {
                    //DO DAMAGE TO PLAYER
                    col.GetComponent<Health>().DecreaseHealth(1);
                    
                    if (hasKnockback)
                    {
                        col.GetComponent<CharecterAttackController>().Hurt((col.transform.position - animator.transform.position).normalized, knockbackForce, knockbackTime);
                    }
                    else
                    {
                        col.GetComponent<CharecterAttackController>().Hurt();

                    }

                    //col.GetComponent<SpriteRenderer>().color = Color.red;
                    //TELL THE SCRIPT TO STOP MOVING
                    hasHitPlayerOnce = true;

                    //PLAY audioFor stab
                    if (stabAudio != null) { stabAudio.Play(animator.GetComponent<AudioSource>()); }

                    //Spawn hit effect
                    RaycastHit2D Ray = Physics2D.Raycast(animator.transform.position, (col.transform.position - animator.transform.position).normalized);
                    Instantiate(hitEffect, new Vector3(Ray.point.x, Ray.point.y, -5), Quaternion.identity, col.transform);

                    //Play certain behaviour parameters if player is hit
                    if(behaviourOnPlHit.Length!=0)
                    for (int i = 0; i < behaviourOnPlHit[randomChoiceForHit].moveParameters.Length; i++)//Set the animator parameters set in the editor
                    {
                        animator.SetBool(behaviourOnPlHit[randomChoiceForHit].moveParameters[i].ParameterName, behaviourOnPlHit[randomChoiceForHit].moveParameters[i].Value);
                    }
                }
            }
        }
        if(effectBeforeAttack!=null)
        if (timer > timeTillEffectBeforeAttackSpawns&&helper==null)
        {

                helper= Instantiate(effectBeforeAttack, colliderTr.position, Quaternion.identity);
        }
        if (timer > timeTillAttackMoves&&timer<timeTillAttackChecksStops)
        {
            rigidbody.MovePosition(animator.transform.position + (dirToPlayer * Time.deltaTime * attackMovementSpeed));
        }
        //For testing the timing
        /* if (timer > timeTillAttackChecksStops)
         {
             animator.GetComponent<SpriteRenderer>().color = Color.green;
         }*/

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasHitPlayerOnce = false;
        hasPlayedswingAudio = false;



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
