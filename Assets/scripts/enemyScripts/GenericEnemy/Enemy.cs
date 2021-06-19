using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] AudioEvent gruntSoundEvent;
    Animator anim;
    [HideInInspector]public Vector3 helperDirToPlayer;


    [Header(" Moves states on which hurt won't happen")]
    [SerializeField] AttackMoveParameter[] attackMovesOnWhickHurtWontHappen;
    [Header(" Parameter varaibles for hurt ")]
    [SerializeField] AttackMoveParameter[] parametersForHurt;



    Vector3 dirToPlayer;

    Vector3 helperPos;

    Transform player;
    Rigidbody2D rb;

    public bool isIn4Directions=false;
    public bool none = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //FLIP the sprite towards the player
        if (none = true)
        {
            if (!isIn4Directions)
            {
                dirToPlayer = (player.position - transform.position).normalized;

                if ((dirToPlayer.x < 0 && transform.lossyScale.x > 0) || (dirToPlayer.x > 0 && transform.lossyScale.x < 0))
                {
                    transform.localScale = new Vector3(transform.lossyScale.x * -1, transform.lossyScale.y, transform.lossyScale.z);
                }
            }
            if (isIn4Directions)
            {
                if (helperPos != transform.position && Vector2.Distance(transform.position, helperPos) > 0.2f)
                {
                    Vector3 dir = (transform.position - helperPos).normalized;
                    Debug.Log(dir + "IS THE DIRECTION");
                    helperPos = transform.position;
                    anim.SetFloat(Animator.StringToHash("VelocityX"), dir.x);
                    anim.SetFloat(Animator.StringToHash("VelocityY"), dir.y);
                }
            }
        }




    }
  
    public void doStatusEffect(AttackStatusEffects attackStatusEffects)
    {

        StartCoroutine(attackStatusEffects.DealEffect(this.gameObject));
    }
    public void Hurt()
    {
        gruntSoundEvent.Play(this.GetComponent<AudioSource>());
        for (int i = 0; i < attackMovesOnWhickHurtWontHappen.Length; i++)
        {
            if( anim.GetBool( Animator.StringToHash(attackMovesOnWhickHurtWontHappen[i].ParameterName) )==true)
            {
                Debug.Log("ON Return");
                return;
            }
        }
        for (int i = 0; i < parametersForHurt.Length; i++)
        {
            anim.SetBool(Animator.StringToHash(parametersForHurt[i].ParameterName), parametersForHurt[i].Value);
        }
       


    }
    
    public void Hurt(Vector2 knockbackDir, float forceAmount, float time)
    {
        if(gruntSoundEvent!=null)
        gruntSoundEvent.Play(this.GetComponent<AudioSource>());
        for (int i = 0; i < attackMovesOnWhickHurtWontHappen.Length; i++)
        {
            if (anim.GetBool(Animator.StringToHash(attackMovesOnWhickHurtWontHappen[i].ParameterName)) == true)
            {
                Debug.Log("ON Return");
                return;
            }
        }
        for (int i = 0; i < parametersForHurt.Length; i++)
        {
            anim.SetBool(Animator.StringToHash(parametersForHurt[i].ParameterName), parametersForHurt[i].Value);
            Debug.Log("Set " + parametersForHurt[i].ParameterName + "To " + parametersForHurt[i].Value);
        }

        StartCoroutine(Knockback(knockbackDir, forceAmount, time));
    }
    public void getDirToPlayer()
    {
        helperDirToPlayer = dirToPlayer;
        
    }
   
    IEnumerator Knockback(Vector2 dir, float speed, float time)
    {
        float timer = Time.time + time;
        Vector2 calcDir;
        while (Time.time < timer)
        {
            calcDir = dir * Time.deltaTime * speed;
            rb.MovePosition(new Vector2(transform.position.x + calcDir.x, transform.position.y + calcDir.y));
            yield return null;
        }
    }
}

