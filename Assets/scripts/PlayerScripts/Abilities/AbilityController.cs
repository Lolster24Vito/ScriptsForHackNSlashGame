using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    [SerializeField] Transform atPos;
    [SerializeField] Ability ability;
    
    public bool isCasting=false;
    // Start is called before the first frame update
    void Start()
    {
        ability.Initialize(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
           
           StartCoroutine( ability.TriggerAbility(this.gameObject));
        }
    }
}
