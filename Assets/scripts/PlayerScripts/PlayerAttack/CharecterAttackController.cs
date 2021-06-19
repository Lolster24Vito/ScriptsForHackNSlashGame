using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharecterAttackController : MonoBehaviour
{
    //Script that controls attacks from player

    
    [SerializeField] Transform atPos;     //atPos is basically where you want the collider check for enemies to be
    [SerializeField] LayerMask attackableLayer;//Layer for attackable stuff
    [SerializeField] GameObject hitEffect;  //A effect that spawns on hits
    [SerializeField] float lenghtTillAutomaticallySnapToEnemy;
 
    float timerHelper;
    [SerializeField]float timeTillComboReset;    //The amount of seconds the game will wait for the player to continue the combo
    //counts on which combo number the player is at
    int comboCounter =0;

    [SerializeField] AudioEvent audioForSwing;
    [SerializeField] AudioEvent audioForStab;
    [SerializeField] AudioEvent audioForHurt;


    //Player Attacks
    [Header("Attack combo that the player can use")]
   [SerializeField] public Attack[] at1;

    [HideInInspector] public bool isHurt = false;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isAtLastFrameOfAttack = false;
    [HideInInspector] public bool isAnimationFinished = false;

    charecterMovementController movScript;
    RotatorOfCollider colliderRotatorScript;

    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sp;
    Vector2 mouseDir;

    //this is a helper variable that stops the waitForSeconds corutine from stacking
    Coroutine randoCorutine=null;
    Coroutine randoHelperCorutine = null;


    void Start()
    {
        //Get all the components from the gameobject that are needed
        Physics2D.queriesStartInColliders = false;
        anim = transform.GetComponent<Animator>();
        sp= transform.GetComponent<SpriteRenderer>();
        movScript = this.GetComponent<charecterMovementController>();
        rb = this.GetComponent<Rigidbody2D>();
        colliderRotatorScript = transform.GetComponentInChildren<RotatorOfCollider>();

    }

    // Update is called once per frame
    void Update()
    {
      

        //If is hurt restart the combo,and set so the player isn't attacking
        if (isHurt)
        {
            isAttacking = false;
            isAtLastFrameOfAttack = false;
            anim.SetBool("isAtLastFrameOfAttack", isAtLastFrameOfAttack);
            anim.SetBool("isAttacking", isAttacking);
            anim.SetBool("isHurt", isHurt);
            comboCounter = 0;

        }

        if (isAttacking)
        {
            checkIfStillAttacking();

        }
        //IF at last frame and standing still...and time runs out return to the idle 
        if (isAtLastFrameOfAttack && Time.time > timerHelper)
        {
            isAtLastFrameOfAttack = false;
            anim.SetBool("isAtLastFrameOfAttack", isAtLastFrameOfAttack);
            comboCounter = 0;

        }

        //Attack if button is pressed and if isn't hurt and  if currently isn't in dodge
        if (Input.GetMouseButtonDown(0)&&!isHurt&&!movScript.isDodging)
        {
            //Added !isHurt because of a bug where if left click is clicked while hurt it would bug out and reset the whole damage animation
            //stop moving
            if (movScript.isMoving)
            {

                movScript.isMoving = false;
            }
            //increase the combo if the timer hasn't run out or if the combo isn't finished
            if (Time.time < timerHelper && comboCounter <= at1.Length - 1)
            {

                //only increase combo at the end of the animation
                if (!isAttacking)
                {
                    comboCounter++;

                }
            }
            else
            {
                comboCounter = 0;

            }


            if (!isAttacking)
            {
                //ok so this line kinda fixes a glitch where the last player combo wouldn't happen if moving until the comboTimer runned out
                //I have no idea why this fixes it but it does and I dare not oppose the might of the C# gods
                if (comboCounter == at1.Length) comboCounter = 0;

                //But basically this is where the attack is called to happen
                mouseDir = GetMouseDir();
                anim.SetFloat("AttackDirX", mouseDir.x);
                anim.SetFloat("AttackDirY", mouseDir.y);

                //Set animation Parameters
                setAnimatorParameters(at1[comboCounter].attackParametersForAnimator);

                  //Play sound
                audioForSwing.Play(this.GetComponent<AudioSource>());

                //Basic bool setting up
                at1[comboCounter].wasLastFrameWaitDone = false;
                isAttacking = true;
                anim.SetBool("isAttacking", isAttacking);
                timerHelper = Time.time + timeTillComboReset;
                //Rotates the "attack transform check collider" to face the mouse
                colliderRotatorScript.RotateCollider();

                //prevents from two moveCorutines for movement while attacking being called at once which results in a bug where the player moves in a first corutine and then second
                if (randoHelperCorutine != null) {
                    StopCoroutine(randoHelperCorutine);
                }

                //Move while attacking
                randoHelperCorutine=StartCoroutine(moveWhileAttacking(mouseDir));


            }

        }

    }
    
    private void checkIfStillAttacking()
    {
        //if the time till next combo hasn't run out and if the animation isnt finished ,it's still attacking
        if (Time.time < timerHelper    &&  !isAnimationFinished)
        {
            isAttacking = true;
            isAtLastFrameOfAttack = false;

        }
        //if animation is done and the time till next combo hasn't run out 
        if (isAnimationFinished && Time.time < timerHelper)
        {
            //added so to let the last frame linguers and so that the attack colliders always appear when you click
            if (at1[comboCounter].wasLastFrameWaitDone)
            {
                isAttacking = false;
                isAtLastFrameOfAttack = true;
             //   sp.color = Color.green;
            }
            else if (!at1[comboCounter].wasLastFrameWaitDone)
            {
                //a corutine that just waits for a milisecond or two and then tells the code that it can move on after the animation.This prevents spamming the attack button
                //added the randoCorutine variable because of the bug where the corutine would run multiple times and stack up causing the waitAtLastFrame to last longer than needed.
                if (randoCorutine==null)
                randoCorutine=StartCoroutine(waitForSec());
            }

        }
        //If combo  time has run out you aren't attacking
        if (Time.time > timerHelper)
        {
            isAttacking = false;
            isAtLastFrameOfAttack = false;
            comboCounter = 0;
        }
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("isAtLastFrameOfAttack", isAtLastFrameOfAttack);
      //  Debug.Log("isAttacking=" + isAttacking + " isAtLastFrameOfAttack=" + isAtLastFrameOfAttack);


    }
    IEnumerator waitForSec()    //Waits for a certain time at the last frame
    {
        checkIfAnythingHit();
        at1[comboCounter].wasLastFrameWaitDone = false;
        //   sp.color = Color.black;
        yield return new WaitForSeconds(at1[comboCounter].attackWait);
        //   sp.color = Color.white;



        at1[comboCounter].wasLastFrameWaitDone = true;
        randoCorutine = null;


    }
    IEnumerator moveWhileAttacking(Vector2 dir)
    {
        float distance=at1[comboCounter].attackMoveDistance;
        float attackMoveSpeed= at1[comboCounter].attackMoveSpeed;
        anim.SetFloat(Animator.StringToHash("XInput"), dir.x);

        //A script that nudges the player towards the enemy if he is too far and assures that the player stays on target if directly in front of enemy
        RaycastHit2D col = Physics2D.CircleCast(transform.position, 0.5f, dir, lenghtTillAutomaticallySnapToEnemy, attackableLayer);
        if (col.collider != null)
        {
            distance = Vector2.Distance(transform.position, col.transform.position);
            attackMoveSpeed = (distance / (0.84f * at1[comboCounter].attackWait * 10)) * 10f;
            if (distance >= 1f) distance -= 1f;
            else attackMoveSpeed = 0;

            Debug.Log("did the calcualtions and attackMoveSpeed=" + attackMoveSpeed + "distance=" + distance);
        }

        Vector2 oldPos = transform.position;
        Vector2 calcDir;
        rb.velocity = Vector2.zero;
        //Stop moving if a certain distance is reached or if player is no longer attacking/lastFrame
        while (Vector2.Distance(oldPos, transform.position) <= distance && (isAttacking||isAtLastFrameOfAttack))
        {
            calcDir = dir * Time.deltaTime * attackMoveSpeed;
            rb.MovePosition(new Vector2(transform.position.x + calcDir.x, transform.position.y + calcDir.y));
            yield return null;
        }
        randoHelperCorutine = null;
    }
    IEnumerator knockback(Vector2 dir,float speed,float time)   //Enemy calls this IEnumerator to call for knockback
    {
        float timer = Time.time+time;
        Vector2 oldPos = transform.position;
        Vector2 calcDir;
        anim.SetFloat(Animator.StringToHash("AttackDirX"), -dir.x);
        //Stop knockback if certain time is reached
        while (Time.time<timer)
        {
            calcDir = dir * Time.deltaTime * speed;
            rb.MovePosition(new Vector2(transform.position.x + calcDir.x, transform.position.y + calcDir.y));
            yield return null;
        }
    }
    void checkIfAnythingHit()//A function that checks if anything is hit
    {
        Collider2D[] col = Physics2D.OverlapBoxAll(atPos.position, new Vector2(at1[comboCounter].xColsize, at1[comboCounter].yColsize),atPos.rotation.eulerAngles.z, attackableLayer);
        foreach(Collider2D colliders in col)
        {
            if (colliders.GetComponent<Enemy>()!=null)
            {

                if (at1[comboCounter].hasKnockback)
                {
                    colliders.GetComponent<Enemy>().Hurt(GetMouseDir(),at1[comboCounter].knockbackForce,at1[comboCounter].knockbackTime);

                }
                else
                {
                    colliders.GetComponent<Enemy>().Hurt();
                }

                audioForStab.Play(this.GetComponent<AudioSource>());
                //Spawn hit effect where it needs to be(by spawning a ray and spawning the HitEffect on the Point(Collision) of the player and enemy
                RaycastHit2D Ray = Physics2D.Raycast(transform.position, (colliders.transform.position - transform.position));
                Instantiate(hitEffect,new Vector3(Ray.point.x,Ray.point.y,-5),Quaternion.identity);
               
                colliders.GetComponent<Health>().DecreaseHealth(1);
                StartCoroutine(flashWhiteForAsec(colliders.gameObject));

            }
        }
    }

    IEnumerator flashWhiteForAsec(GameObject ob)
    {
        SpriteRenderer sp = ob.GetComponent<SpriteRenderer>();
        sp.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        sp.color = Color.white;

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(atPos.position,new Vector3(at1[comboCounter].xColsize,at1[comboCounter].yColsize,0));
    }


    Vector2 GetMouseDir()
    {
       Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseDir = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized;
       // Debug.Log(mouseDir+"Old");

      //  Debug.Log(mouseDir + "new");

        return mouseDir;

    }

    public void Hurt(Vector3 dir,float forceAmount,float knockbackTime)
    {
        //basic bool setup
        isHurt = true;
        isAttacking = false;
        isAtLastFrameOfAttack = false;
        anim.SetBool("isAtLastFrameOfAttack", isAtLastFrameOfAttack);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("isHurt", isHurt);

        //hurt resets the combo
        comboCounter = 0;

        StartCoroutine(knockback(dir, forceAmount, knockbackTime));
        Debug.Log("I AM DOING THE HURT dir=" + dir + " forceamount=" + forceAmount);

        //Play audio for hurt
        if (audioForHurt != null) audioForHurt.Play(this.GetComponent<AudioSource>());
    }
    public void Hurt()
    {
        isHurt = true;
        isAttacking = false;
        isAtLastFrameOfAttack = false;

        anim.SetBool("isAtLastFrameOfAttack", isAtLastFrameOfAttack);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("isHurt", isHurt);

        comboCounter = 0;
        //Play audio for hurt
        if (audioForHurt != null) audioForHurt.Play(this.GetComponent<AudioSource>());
    }
    public  int GetComboCounter 
    {
        get
        {
            return comboCounter;
        }
       
    }
    void setAnimatorParameters(AttackMove attackParameters)
    {
            for (int i = 0; i < attackParameters.moveParameters.Length; i++)//Set the animator parameters set in the editor
            {
                anim.SetBool(attackParameters.moveParameters[i].ParameterName, attackParameters.moveParameters[i].Value);
            }
    }



    public void SetSwordInHand(bool swordInHandBool)
    {
        anim.SetBool(Animator.StringToHash("SwordInHand"), swordInHandBool);
    }


}


