using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EventIfTouchesTrigger : MonoBehaviour
{
    [SerializeField]UnityEvent events;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            events.Invoke();
        }
        
    }
}
