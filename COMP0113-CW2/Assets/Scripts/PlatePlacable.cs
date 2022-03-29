using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using UnityEngine;

/* Used for bun gameobject to ensure it is centered on a plate when it touches one. 
 * Transform parent is set to plate when placed on one and set to world when grasped. (Needed for checking in serving Tray)
 */
public class PlatePlacable : MonoBehaviour
{
    private Vector3 newPosition;
    private int counter = 0; 

    void OnCollisionEnter(Collision other)
    {
        if ((other.gameObject.name == "Plate" || other.gameObject.name == "SmallPlate") & counter == 0)
        {
            gameObject.transform.parent = other.gameObject.transform;

            //float y_offset = gameObject.GetComponent<Collider>().bounds.size.y / 2 + other.gameObject.GetComponent<Collider>().bounds.size.y / 2;
            //newPosition = other.gameObject.transform.position;
            //newPosition.y += 0.3f;


            gameObject.transform.localPosition = new Vector3(0, 0.3f, 0);
            counter += 1;
            //gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    // Checks if object is AttachedGraspable or Graspable and returns "grasped" variable from relevant script
    Hand GraspStatus()
    {
        if (gameObject.GetComponent<Graspable>() != null)
        {
            return gameObject.GetComponent<Graspable>().grasped;
        }
        else if (gameObject.GetComponent<AttachedGraspable>() != null)
        {
            return gameObject.GetComponent<AttachedGraspable>().grasped;
        }
        else
        {
            return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GraspStatus())
        {
            gameObject.transform.parent = null;
            counter = 0;
        }
    }
}
