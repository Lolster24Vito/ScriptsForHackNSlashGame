using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScriptForRotationalMovement : MonoBehaviour
{
    public float angle = 0.1f;
    public Transform pl;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            transform.position = getRotationPositions(angle);

        }
    }
    Vector3 getRotationPositions(float angle)
    {
        //randomly get -1 or 1
        // int t = Random.Range(0, 2) * 2 - 1;
        int t = 1;
            Vector3 dif = transform.position - pl.position;
            float x1 = (dif.x * Mathf.Cos(angle * t)) - (dif.y * Mathf.Sin(angle * t));
            float y1 = (dif.y * Mathf.Cos(angle * t)) + (dif.x * Mathf.Sin(angle * t));
           // transform.position = new Vector3(x1+pl.position.x, y1+pl.position.y, -1);
             return new Vector3(x1 + pl.position.x, y1 + pl.position.y, -1);
        
    }
}
