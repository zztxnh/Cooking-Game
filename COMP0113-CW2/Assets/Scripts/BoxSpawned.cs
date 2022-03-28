using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Ubiq.XR;
using Ubiq.Samples;
using UnityEngine;

/* Deals with the networking of the sapawned objects from a box.
 * Deals with the following if a message is received from the owner (owner ius changed whenever the object is grasped)
 *  - Changes transform to match owner 
 *  - Changes boolean destroy variable which destroys the object afterwards
 *  - Changes transform parent to match owner
 *  - Changes isKinematic status to match owner
 */
[RequireComponent(typeof(Rigidbody))]
public class BoxSpawned : MonoBehaviour, IBox, ISpawnable, INetworkObject, INetworkComponent
{
    private NetworkContext context;
    private bool grasped;

    public NetworkId Id { get; set; }
    public bool owner; // Used by other scripts
    public bool destroy; // Used by other scripts

    public struct Message
    {
        public TransformMessage transform;
        public bool destroy;
        public bool isKin;
        public string parentName;

        public Message(Transform transform, bool destroy, bool isKin, string parentName)
        {
            this.transform = new TransformMessage(transform);
            this.destroy = destroy;
            this.isKin = isKin;
            this.parentName = parentName;
        }
    }

    void IBox.Attach(Hand hand)
    {
        gameObject.transform.position = hand.transform.position;
        gameObject.transform.rotation = hand.transform.rotation;
        gameObject.GetComponent<Graspable>().Grasp(hand);
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
        gameObject.transform.parent = GameObject.Find(msg.parentName).transform;
        gameObject.transform.localPosition = msg.transform.position; // The Message constructor will take the *local* properties of the passed transform.
        gameObject.transform.localRotation = msg.transform.rotation;
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
            bool isKin = gameObject.GetComponent<Rigidbody>().isKinematic;
            string parentName;
            if(gameObject.transform.parent != null)
            {
                parentName = gameObject.transform.parent.name;
            }
            else
            {
                parentName = "Scene Manager";
            }
            context.SendJson(new Message(gameObject.transform, destroy, isKin, parentName));
        }
        if (destroy)
        {
            Destroy(gameObject);
        }
    }
}

