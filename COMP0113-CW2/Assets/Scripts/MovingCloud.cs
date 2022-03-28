using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCloud : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public float speed = 1;
    void Update()
    {
        // Moves an object forward, relative to its own rotation.
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
