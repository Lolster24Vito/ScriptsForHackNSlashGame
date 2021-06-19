using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 3;
    int idDeath = Animator.StringToHash("Death");

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DecreaseHealth(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Death();
        }
    }
    void Death()
    {
        foreach(BoxCollider2D collider2D in this.GetComponentsInChildren<BoxCollider2D>())
        {
            collider2D.enabled = false;
        }
        foreach (CircleCollider2D collider2D in this.GetComponentsInChildren<CircleCollider2D>())
        {
            collider2D.enabled = false;
        }
        this.GetComponent<Animator>().SetBool(idDeath, true);
        this.GetComponent<Collider2D>().enabled = false;
        if(this.GetComponentInChildren<CircleCollider2D>()!=null)
        this.GetComponentInChildren<CircleCollider2D>().enabled = false;
        if (this.GetComponentInChildren<BoxCollider2D>()!=null)
        {
            this.GetComponentInChildren<BoxCollider2D>().enabled = false;

        }
        if (this.GetComponent<Enemy>())
        {
            this.GetComponent<Enemy>().enabled = false;
        }

    }
}
