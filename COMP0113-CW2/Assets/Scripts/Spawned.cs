using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Ubiq.XR;
using Ubiq.Samples;
using UnityEngine;

/* Deals with the networking of the spawned object 
 * (Should not be used to object that need to spawn grasped)
 */
public class Spawned : MonoBehaviour, ISpawnable, INetworkObject, INetworkComponent
{
    private GameObject SpawnedFrom;
    private string vegBoxName;
    private NetworkContext context;
    private bool grasped;

    public NetworkId Id { get; set; }
    public bool owner; // Used by other scripts
    public bool destroy; // Used by other scripts

    public struct Message
    {
        public TransformMessage transform;
        public bool isKin;
        public bool destroy; 

        public Message(Transform transform, bool destroy, bool isKin)
        {
            this.transform = new TransformMessage(transform);
            this.destroy = destroy;
            this.isKin = isKin;
        }
    }

    void ISpawnable.OnSpawned(bool local)
    {
        owner = local;
        gameObject.GetComponent<Rigidbody>().isKinematic = !local;
    }

    void Awake()
    {
        owner = false;
        destroy = false;

        transform.name = transform.name.Replace("(Clone)", "");
        context = NetworkScene.Register(this);
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        owner = false;
        var msg = message.FromJson<Message>();
        transform.localPosition = msg.transform.position; // The Message constructor will take the *local* properties of the passed transform.
        transform.localRotation = msg.transform.rotation;
        destroy = msg.destroy;
        gameObject.GetComponent<Rigidbody>().isKinematic = msg.isKin;
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
        grasped = GraspStatus();
        if (grasped)
        {
            owner = true;
        }
        if (owner)
        {
            context.SendJson(new Message(transform, destroy, gameObject.GetComponent<Rigidbody>().isKinematic));
        }
        if (destroy)
        {
            Destroy(gameObject);
        }
    }
}

