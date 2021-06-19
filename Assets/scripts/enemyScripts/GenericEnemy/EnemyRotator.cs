using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotator : MonoBehaviour
{
    //A script that rotates to in direction to something or gets dirToPlayer
    //It's used on a parent that has a child that should rotate

    Transform player;
    Transform enemy;
    [SerializeField] float distance;
    public Vector3 difference;

    float angle;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
      
        
    }
    public Vector3 getDirToPlayer()
    {
        Vector3 difference = player.position - enemy.position;
        return difference;

    }
    public Vector3 getDirToPlayer(Transform tr)
    {
        Vector3 difference = player.position - tr.position;
        return difference;

    }
    public void rotateToPlayer()
    {
        Vector3 difference = player.position - enemy.position;
        //Find angle
         angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        
        //Rotate around player if flipped to the right
        if (transform.lossyScale.x > 0)
        {
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        //Rotate around player if flipped to the left

        if (transform.lossyScale.x < 0)
        {
            transform.rotation = Quaternion.AngleAxis(angle + 180f, Vector3.forward);
        }
    }
   
    //if distance changes automatically change the position
    void OnValidate()
    {
        transform.GetChild(0).localPosition = new Vector3(distance, 0, 0);
    }
}
