using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorOfCollider : MonoBehaviour
{
    //A script that rotates a AttackPositon Transform around the player when a attack button is pressed

    //Distance between the player and the Aimer
    [SerializeField] float distance = 3.0f;
    public static float angle = 0f;
    [SerializeField] private bool alwaysRotate = false;
    Camera cam;
    void Start()
    {
        //The position of the child is always V3(0,1,0) so we multiply it by the distance
        transform.GetChild(0).localPosition = new Vector3(distance, 0, 0);
        cam = Camera.main;
    }

    void Update()
    {
        if (alwaysRotate)
        {
            RotateCollider();

        }
        //Find The player on screen point
    }

   public  void RotateCollider()
    {
        Vector3 v3Pos = cam.WorldToScreenPoint(transform.parent.position);
        v3Pos = Input.mousePosition - v3Pos;
        //Find angle
        angle = Mathf.Atan2(v3Pos.y, v3Pos.x) * Mathf.Rad2Deg;
        //Rotate around player
        Debug.Log(transform.lossyScale + "lossy");
        //Rotate around player if flipped to the right
        if (transform.lossyScale.x > 0)
        {
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        //Rotate around player if flipped to the left
 
        if (transform.lossyScale.x < 0)
        {
            transform.rotation = Quaternion.AngleAxis(angle+180f, Vector3.forward);
        }
    }
    
    //if distance changes automatically change the position
    void OnValidate()
    {
        transform.GetChild(0).localPosition = new Vector3(distance, 0, 0);
    }
}
