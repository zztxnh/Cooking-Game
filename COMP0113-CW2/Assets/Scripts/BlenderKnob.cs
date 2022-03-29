using System.Collections;
using System.Collections.Generic;
using Ubiq.XR;
using Ubiq.Samples;
using UnityEngine;

public class BlenderKnob : MonoBehaviour, IUseable
{
    public GameObject Lid; // To be set from Inspector
    public bool Blended;

    private List<GameObject> vegs = new List<GameObject>();
    private string JuiceName;
    private GameObject JuicePrefab;

    void IUseable.UnUse(Hand controller)
    {

    }

    void IUseable.Use(Hand controller)
    {
        vegs = new List<GameObject>(Lid.GetComponent<BlenderLid>().vegs);
        if (vegs.Count >= 2)
        {
            StartCoroutine(Blend());
        }
    }

    IEnumerator Blend()
    {
        Lid.GetComponent<BlenderLid>().vegs = new List<GameObject>();
        yield return new WaitForSeconds(5);
        foreach (GameObject Object in vegs)
        {
            Object.GetComponent<Spawned>().destroy = true;
        }
        GameObject BlenderJuice = NetworkSpawner.Spawn(this, JuicePrefab);
        BlenderJuice.transform.position = Lid.transform.position;
        Blended = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Blended = false;
        JuiceName = "BlenderJuice";
        JuicePrefab = (GameObject)Resources.Load(JuiceName, typeof(GameObject));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
