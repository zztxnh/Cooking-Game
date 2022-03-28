using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using UnityEngine;

/* Gives grasp capablities to the object whilst maintaining object's starting position when released
 * - isKinematic always true 
 */
[RequireComponent(typeof(Rigidbody))]
public class AttachedGraspable : MonoBehaviour, IGraspable
{
    private Vector3 startPos;
    private Quaternion startRot;
    private Vector3 localGrabPoint;
    private Quaternion localGrabRotation;
    private Quaternion grabHandRotation;
    private Transform follow;

    public Hand grasped; // Used by other scripts

    void IGraspable.Grasp(Hand controller)
    {
        var handTransform = controller.transform;
        localGrabPoint = handTransform.InverseTransformPoint(transform.position); //transform.InverseTransformPoint(handTransform.position);
        localGrabRotation = Quaternion.Inverse(handTransform.rotation) * transform.rotation;
        grabHandRotation = handTransform.rotation;
        follow = handTransform;

        grasped = controller;
    }

    public void Release(Hand controller) // Used by Networkable.cs
    {
        follow = null;
        grasped = null;
        transform.localPosition = startPos;
        transform.localRotation = startRot;
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localRotation;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
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
