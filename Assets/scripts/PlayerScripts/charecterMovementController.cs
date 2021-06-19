using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charecterMovementController : MonoBehaviour
{
    //A Script that does player movement


   [HideInInspector] public  bool isMoving;
   [HideInInspector] public bool isDodging;
    private bool isDodgeAvailable;


    [Tooltip("The speed the animation will be played at")]
    [Range(0, 1)]
    [SerializeField] float runAnimationSpeed;
    [Tooltip("The speed at which the player will move at")]
    [SerializeField]  float speed;
    [Tooltip("The speed of the dash")]
    [SerializeField] float dashSpeed;
    [Tooltip("The lenght of the dash")]
    [SerializeField] float dashTime;
    [Tooltip("The cooldown of the dash")]
    [SerializeField] float dashCooldownTime;

    CharecterAttackController attackScript;
    AbilityController abilityCScript;

    int animX = Animator.StringToHash("XInput");
    int animY = Animator.StringToHash("YInput");

    Rigidbody2D rb;
    Vector2 input;
    float MovDirForScale=1;
    Animator anim;
    private Vector3 scale;
    // Start is called before the first frame update
   
    void Start()
    {
    rb = transform.GetComponent<Rigidbody2D>();
        anim = transform.GetComponent<Animator>();
        attackScript = this.GetComponent<CharecterAttackController>();
        abilityCScript = this.GetComponent<AbilityController>();
        anim.SetFloat("runAnimationSpeed", runAnimationSpeed);
        scale = transform.localScale;
        isDodgeAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //IF the button is pressed and is not attacking and the dodge isn't in cooldown you can dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !attackScript.isAttacking&&isDodgeAvailable&&!abilityCScript.isCasting)
        {
            StartCoroutine(Dash());
        }

    }
    private void FixedUpdate()
    {
        //THE PLAYER CAN'T MOVE AND ATTACK AT THE SAME TIME,AND CAN'T MOVE IF HE IS HURT,OR CASTING A ABILITY
        if (!attackScript.isAttacking&&!attackScript.isHurt&&!isDodging&&!abilityCScript.isCasting)
        {
           
                Move();
               
            //This fixes a animation bug that waits for the isAtLastFrameAttack till going to idle

            if (attackScript.isAtLastFrameOfAttack&&input.magnitude!=0)
            {
                attackScript.isAtLastFrameOfAttack = false;
                anim.SetBool("isAtLastFrameOfAttack", attackScript.isAtLastFrameOfAttack);
            }
        }
        else
        {
            isMoving = false;
            anim.SetBool("isMoving", isMoving);
        }
        if (attackScript.isAttacking)
        {
            
        }
    }
    IEnumerator  Dash()
    {
        //Set the parameters for the animator
        anim.SetFloat(animX, input.x);
        anim.SetFloat(animY, input.y);

        isDodgeAvailable = false;
        isDodging = true;

            rb.velocity = Vector2.zero;
            rb.velocity = new Vector2(input.x, input.y) * dashSpeed;

        anim.SetBool("isDodging", isDodging);
        anim.SetBool("isMoving", false);

        transform.GetComponent<TrailRenderer>().enabled = true;
        transform.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(dashTime);
        transform.GetComponent<TrailRenderer>().enabled = false;
        transform.GetComponent<BoxCollider2D>().enabled = true;


        rb.velocity = Vector2.zero;
        isDodging = false;
        anim.SetBool("isDodging", isDodging);


        yield return new WaitForSeconds(dashCooldownTime);
        isDodgeAvailable = true;



    }
    private void Move()
    {

      Vector2 mov = input.normalized * Time.fixedDeltaTime * speed;
        mov = new Vector2(mov.x + transform.position.x, mov.y + transform.position.y);

        rb.MovePosition(mov);
        //set the parameters for the animator

        if (input.magnitude != 0) {
            isMoving = true;
            anim.SetBool("isMoving", isMoving);
            anim.SetFloat(animX, input.x);
            anim.SetFloat(animY, input.y);
        }
        else {
            isMoving = false;

            anim.SetBool("isMoving", isMoving); }
        
    }
  
}
