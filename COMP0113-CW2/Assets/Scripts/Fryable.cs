using System.Collections;
using System.Collections.Generic;
using Ubiq.Samples;
using UnityEngine;

public class Fryable : MonoBehaviour
{
    private GameObject CookedPrefab;
    private string CookedName;
    private int counter = 0;

    public AudioSource audiosource;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "FryingPan" & counter == 0 & gameObject.GetComponent<Spawned>().owner)
        {
            counter += 1;
            gameObject.transform.position = other.gameObject.transform.position + new Vector3(0, 0.15f, 0);
            StartCoroutine(Cook());
        }
    }

    void Awake()
    {
        CookedName = "Cooked" + transform.name.Replace("(Clone)", "");
        CookedPrefab = (GameObject)Resources.Load(CookedName, typeof(GameObject));
    }

    IEnumerator Cook()
    {
        audiosource.Play();
        yield return new WaitForSeconds(5);
        audiosource.Stop();

        GameObject cookedObj = NetworkSpawner.Spawn(this, CookedPrefab);
        cookedObj.transform.position = gameObject.transform.position;
        gameObject.GetComponent<Spawned>().destroy = true;
        counter = 0;
    }
}
