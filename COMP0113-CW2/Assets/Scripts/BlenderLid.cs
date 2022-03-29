using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ubiq.XR;
using Ubiq.Samples;
using UnityEngine;

public class BlenderLid : MonoBehaviour
{
    public GameObject Knob; // To be set from Inspector
    public List<GameObject> vegs = new List<GameObject>();

    private string JuiceGlassName;
    private GameObject JuiceGlassPrefab;

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<Graspable>().grasped & other.gameObject.GetComponent<Spawned>().owner)
        {
            if (other.name == "Lemon")
            {
                vegs.Add(other.gameObject);
            }
            else if (other.name == "Glass")
            {
                if (Knob.GetComponent<BlenderKnob>().Blended)
                {
                    GameObject BlenderJuice = GameObject.Find("BlenderJuice");
                    BlenderJuice.GetComponent<Spawned>().destroy = true;
                    var obj = NetworkSpawner.Spawn(this, JuiceGlassPrefab);//.GetComponents<MonoBehaviour>().Where(mb => mb is IBox).FirstOrDefault() as IBox;
                    if (obj != null)
                    {
                        Vector3 newPosition = gameObject.transform.position;
                        newPosition.z -= 0.2f;
                        obj.transform.position = newPosition;
                        //Hand controller = other.gameObject.GetComponent<Graspable>().grasped;
                        //obj.Attach(controller);
                    }
                    other.gameObject.GetComponent<Spawned>().destroy = true;
                }
            }
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        JuiceGlassName = "LemonGlass";
        JuiceGlassPrefab = (GameObject)Resources.Load(JuiceGlassName, typeof(GameObject));
        vegs.Clear();
    }
}

