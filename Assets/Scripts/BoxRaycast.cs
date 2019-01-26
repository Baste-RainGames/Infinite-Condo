using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxRaycast : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public Transform sightStart, sightEnd;

    public bool stop = false;

    public Transform spawnPos;
    public GameObject spawnee;

    
    // Update is called once per frame
    void Update()
    {
        Raycasting();
        Behaviors();
    }

    void Raycasting()
    {
        Debug.DrawLine(sightStart.position, sightEnd.position, Color.green);

        stop = Physics2D.Linecast(sightStart.position, sightEnd.position);

        if (stop == true) 
        {
            transform.position += Vector3.up;

            

            Instantiate(spawnee, spawnPos.position, spawnPos.rotation);

        }
    }

    void Behaviors()
    {

    }



}
