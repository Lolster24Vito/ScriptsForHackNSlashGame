using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnHealthDropEvent : MonoBehaviour
{
    Health health;
    Animator animator;
    public int[] onHealthDropAmount;
    public string[] nameOfAnimation;

    public UnityEvent events;
    int counter=0;

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Health>())
        health = GetComponent<Health>();

        if (GetComponent<Animator>())
            animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(int i in onHealthDropAmount) {


        
        if (health.health == onHealthDropAmount[i])
        {
                animator.Play(Animator.StringToHash(nameOfAnimation[i]));

        }


        }
        if (health.health == 0)
        {
            events.Invoke();
        }
    }
}
