using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        Console.WriteLine("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision with Player detected!");
            // You can add more logic here, like playing a sound or triggering an event
        }
    }
}
