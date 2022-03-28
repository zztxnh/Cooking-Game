using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using UnityEngine;

/* Gives grasp capablities to the object 
 * - isKinematic is true when the object is grasped; false otherwise (Changes that of children as well)
 */
[RequireComponent(typeof(Rigidbody))]
public class Graspable : MonoBehaviour, IGraspable
{
    private Rigidbody[] childBodies;
    private Vector3 localGrabPoint;
    private Quaternion localGrabRotation;
    private Quaternion grabHandRotation;
    private Transform follow;

    public Hand grasped; // Used by other scripts

    public void Grasp(Hand controller) // Used by Attach in VegSpawned
    {
        var handTransform = controller.transform;
        localGrabPoint = handTransform.InverseTransformPoint(transform.position); //transform.InverseTransformPoint(handTransform.position);
        localGrabRotation = Quaternion.Inverse(handTransform.rotation) * transform.rotation;
        grabHandRotation = handTransform.rotation;
        follow = handTransform;

        grasped = controller;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        if (childBodies != null || childBodies.Length != 0)
        {
            for (int i = 0; i < childBodies.Length; i++)
            {
                childBodies[i].isKinematic = true;
            }
        }   
    }

    void IGraspable.Release(Hand controller)
    {
        follow = null;
        grasped = null;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        if (childBodies != null || childBodies.Length != 0)
        {
            for (int i = 0; i < childBodies.Length; i++)
            {
                childBodies[i].isKinematic = false;
            }
        }
    }

    void Awake()
    {
        childBodies = gameObject.GetComponentsInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (follow)
        {
            transform.rotation = follow.rotation * localGrabRotation;
            transform.position = follow.TransformPoint(localGrabPoint);
        }
    }
}
