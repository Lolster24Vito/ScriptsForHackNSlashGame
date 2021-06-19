using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStuffWhenButtonPressed : MonoBehaviour
{
    
    int counter;
    [SerializeField] Transform[] positions;
    [SerializeField] GameObject[] enemySpawner;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (counter == positions.Length) counter = 0;

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)){
            Instantiate(enemySpawner[0], positions[counter].position, Quaternion.identity);
            counter++;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            Instantiate(enemySpawner[1], positions[counter].position, Quaternion.identity);
            counter++;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            Instantiate(enemySpawner[2], positions[counter].position, Quaternion.identity);
            counter++;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            Instantiate(enemySpawner[3], positions[counter].position, Quaternion.identity);
            counter++;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject ob in enemys)
            {
                Destroy(ob);
            }
        }

    }
}
