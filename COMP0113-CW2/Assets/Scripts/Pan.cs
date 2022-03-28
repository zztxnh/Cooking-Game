using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Ubiq.XR;
using Ubiq.Samples;

public class Pan : MonoBehaviour
{
    public AudioSource audiosource;

    private Vector3 newPosition;
    private int counter = 0;
    private GameObject CookedPrefab;
    private string CookedName;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Meat" & counter == 0 & other.gameObject.GetComponent<BoxSpawned>().owner)
        {
            CookedName = "Cooked" + other.gameObject.name.Replace("(Clone)", "");
            CookedPrefab = (GameObject)Resources.Load(CookedName, typeof(GameObject));

            other.transform.parent = gameObject.transform;
            //float y_offset = gameObject.GetComponent<Collider>().bounds.size.y / 2 + other.gameObject.GetComponent<Collider>().bounds.size.y / 2;
            //newPosition = other.gameObject.transform.position;
            newPosition = new Vector3(0, 0.15f, 0);
            other.gameObject.transform.localPosition = newPosition;
            //other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            counter += 1;
            //gameObject.GetComponent<Rigidbody>().isKinematic = true;
            
            StartCoroutine(Cook(other.gameObject));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if meat is still on pan (plate placable script removes the meat from children whenever grasped)
        Transform[] childTransforms = gameObject.GetComponentsInChildren<Transform>();
        if (childTransforms != null || childTransforms.Length != 0)
        {
            counter = 0;
        }
    }

    IEnumerator Cook(GameObject uncookedObj)
    {
        audiosource.Play();
        yield return new WaitForSeconds (5);
        audiosource.Stop();
        GameObject cookedObj = NetworkSpawner.Spawn(this, CookedPrefab);
        cookedObj.transform.position = uncookedObj.transform.position;
        uncookedObj.GetComponent<BoxSpawned>().destroy = true;
    }
}
