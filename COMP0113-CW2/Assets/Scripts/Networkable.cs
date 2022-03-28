using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Ubiq.XR;
using UnityEngine;

/* Gives the object network properties (only used for objects available in the start; not for spawned objects)
 * - NetworkId is set according to object's position
 * - Only sends transform if object is grasped (owner not implemented/basically user who grasped is the owner)
 * - Ensure object is back in place if object is attached
 */
public class Networkable : MonoBehaviour, INetworkObject, INetworkComponent
{
    private NetworkContext context;
    private bool prevGrasped; // Saves the previous grasped status to detect if the object was released (For attachedGrasplabe)

    public NetworkId Id { get; set; }

    public struct Message
    {
        public TransformMessage transform;

        public Message(Transform transform)
        {
            this.transform = new TransformMessage(transform);
        }
    }

    void INetworkComponent.ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var msg = message.FromJson<Message>();
        transform.localPosition = msg.transform.position; // The Message constructor will take the *local* properties of the passed transform.
        transform.localRotation = msg.transform.rotation;
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

    // Start is called before the first frame update
    void Start()
    {
        Vector3 startPosVec = transform.position;
        int id = (int)(System.Math.Floor(startPosVec[0] * 100) +
            System.Math.Floor(startPosVec[1] * 100) +
            System.Math.Floor(startPosVec[2] * 100));
        var networkId = new NetworkId((uint)id); 
        Id = networkId;
        context = NetworkScene.Register(this);

        prevGrasped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (prevGrasped)
        {
            context.SendJson(new Message(transform));
        }
        prevGrasped = GraspStatus();
    }
}
