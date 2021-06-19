using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distance : MonoBehaviour
{
    public Transform player;
   [SerializeField]float DistanceOf;
    [SerializeField] float lerpValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DistanceOf = Vector2.Distance(player.position, transform.position);
        Debug.Log(Vector2.Distance(player.position, transform.position)+"=DISTANCE");
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            Vector2 vect = getCirclePosition();
            transform.position = Vector3.Lerp(new Vector3(vect.x,vect.y,-7),player.position, lerpValue);
        }
    }


    Vector2 getCirclePosition()
    {
      float  angle = Random.Range(0, 360);

        int randDir = Random.Range(0, 2) * 2 - 1;
        float Distounce = 1f;

        float t = randDir;
        Vector3 dif = transform.position - player.position + new Vector3(0.5f, 0.5f, 0) * Distounce;
        float x1 = (dif.x * Mathf.Cos(angle * t)) - (dif.y * Mathf.Sin(angle * t));
        float y1 = (dif.y * Mathf.Cos(angle * t)) + (dif.x * Mathf.Sin(angle * t));

        return new Vector2(x1 + player.position.x, y1 + player.position.y);
    }
    int GetPosOrNegNumb()
    {
        return Random.Range(0, 2) * 2 - 1;

    }
}
