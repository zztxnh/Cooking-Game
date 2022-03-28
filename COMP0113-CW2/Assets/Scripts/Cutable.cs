using System.Collections;
using System.Collections.Generic;
using Ubiq.Samples;
using UnityEngine;

/* Used for vegetables; 
 * *Attaches veg on Cutting board in casse of contact (while not grasped)
 * *Contact with knife is kept score of. when score reaches 5 gameobject is destroyed and cutveg is spawned
 */
public class Cutable : MonoBehaviour
{
    private string cutVegName;

    public GameObject CutVegPrefab; // Public just to read it on Inspector
    public int Score; // Public just to read it on Inspector
    public AudioSource audiosource;
     
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Knife" & gameObject.GetComponent<BoxSpawned>().owner)
        {
            Score++;
            audiosource.Play();
        }
        if (other.name == "CuttingBoard")
        {
            gameObject.transform.position = other.gameObject.transform.position + new Vector3(0, 0.01f, 0); ;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void Awake()
    {
        Score = 0;
        cutVegName = "Cut" + transform.name.Replace("(Clone)", "");
        CutVegPrefab = (GameObject)Resources.Load(cutVegName, typeof(GameObject));
    }

    // Update is called once per frame
    void Update()
    {
        if (Score >= 5)
        {
            GameObject cutVeg = NetworkSpawner.Spawn(this, CutVegPrefab);
            cutVeg.transform.position = gameObject.transform.position;
            gameObject.GetComponent<BoxSpawned>().destroy = true;
            Score = 0;
        }
    }
}
