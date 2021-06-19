using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;
    [SerializeField] float knockbackPower;
    [SerializeField] float knockbackTime;
    [SerializeField] string attackableTag;
    public int damage=1;
    public AttackStatusEffects atStatusEffect;



    public Vector2 dir;
    public float speed;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dir != null && dir != Vector2.zero)
        {
            
           Vector2 calcDir = dir * Time.deltaTime * speed;
         //   rb.MovePosition(new Vector2(transform.position.x + calcDir.x, transform.position.y + calcDir.y));
            rb.velocity=dir * speed;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HIT Function");
        Debug.Log(collision.gameObject.layer);
        Debug.Log(collision.gameObject.name);

       
            Debug.Log("HIT Layer");
            if (collision.gameObject.CompareTag(attackableTag))
            {

            Debug.Log(collision.gameObject.GetComponent<Health>());
                collision.gameObject.GetComponent<Health>().DecreaseHealth(damage);
            if (atStatusEffect != null)
            {
               StartCoroutine( atStatusEffect.DealEffect(collision.gameObject));
            }

            if(collision.gameObject.GetComponent<CharecterAttackController>() != null)
                collision.gameObject.GetComponentInParent<CharecterAttackController>().Hurt(dir, knockbackPower, knockbackTime);

            if (collision.gameObject.GetComponent<Enemy>() != null)
                collision.gameObject.GetComponent<Enemy>().Hurt(dir, knockbackPower, knockbackTime);

            
        }

            

            Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        
      
    }
}
