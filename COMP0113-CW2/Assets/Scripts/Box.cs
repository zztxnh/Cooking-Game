using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ubiq.XR;
using Ubiq.Samples;
using UnityEngine;

/* Spawns a corresponding object when the box is used 
 * - Has IBox interface which should be implemented in BoxSpawned
 */
public interface IBox
{
    void Attach(Hand hand);
}

public class Box : MonoBehaviour, IUseable
{
    private GameObject ObjPrefab;
    private string ObjName;
    private int count;

    void IUseable.UnUse(Hand controller)
    {
    }

    void IUseable.Use(Hand controller)
    {
        if (count == 0)
        {
            var obj = NetworkSpawner.Spawn(this, ObjPrefab).GetComponents<MonoBehaviour>().Where(mb => mb is IBox).FirstOrDefault() as IBox;
            if (obj != null)
            {
                obj.Attach(controller);
            }
            count = 1;
            StartCoroutine(delay());
        }
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(1f);
        count = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        ObjName = transform.name.Replace("Box", "") ;
        ObjPrefab = (GameObject)Resources.Load(ObjName, typeof(GameObject));
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }
}

