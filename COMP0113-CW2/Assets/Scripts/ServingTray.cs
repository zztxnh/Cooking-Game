using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Checks the components of the plate placed on the serving tray and compares to the requirements of the current order.
 * Calls functions in "OrderManager" script depending on if the served plate is right or wrong.
 * Serving tray should be a child of an empty gameobject "Orders Manager" that will include the orders canvas and anything related to serving.
 */
public class ServingTray : MonoBehaviour
{
    private OrderManager parentScript;
    public AudioSource audiosource;

    void OnCollisionEnter(Collision other)
    {
        
        if ((other.gameObject.name == "Plate" || other.gameObject.name == "SmallPlate") & other.gameObject.GetComponent<Spawned>().owner)
        {
            Transform[] children = other.gameObject.GetComponentsInChildren<Transform>();
            switch(parentScript.currentPanel.name)
            {
                case "Sandwich Panel":
                    List<Transform> loaf = GetChildrenWithName(other.gameObject.transform, "CutLoaf");
                    List<Transform> cheese = GetChildrenWithName(other.gameObject.transform, "CutCutCheese");
                    List<Transform> tomato = GetChildrenWithName(other.gameObject.transform, "CutTomato");
                    Debug.Log("Loaf: " + loaf.Count);
                    Debug.Log("CutCutCheese: " + cheese.Count);
                    Debug.Log("CutTomato: " + tomato.Count);
                    if (loaf.Count == 2 & cheese.Count == 1 & tomato.Count == 1 & children.Length == 5)
                    {
                        audiosource.Play();
                        Debug.Log("Success!!!!!!!!!");
                        parentScript.SuccessOrder(5, other.gameObject.GetComponent<Spawned>().owner);
                        StartCoroutine(MoveObj(other.gameObject));
                    }
                    else
                    {
                        Debug.Log("Wrong Order!!!!!!!!!");
                        parentScript.FailOrder();
                    }
                    break;
                case "Steak Panel":
                    List<Transform> steak = GetChildrenWithName(other.gameObject.transform, "CookedMeat");
                    if (steak.Count == 1 & children.Length == 2)
                    {
                        audiosource.Play();
                        Debug.Log("Success!!!!!!!!!");
                        parentScript.SuccessOrder(10, other.gameObject.GetComponent<Spawned>().owner);
                        StartCoroutine(MoveObj(other.gameObject));
                    } 
                    else
                    {
                        Debug.Log("Wrong Order!!!!!!!!!");
                        parentScript.FailOrder();
                    }
                    break;
                case "LemonJuice Panel":
                    List<Transform> juice = GetChildrenWithName(other.gameObject.transform, "LemonGlass");
                    if (juice.Count == 1)
                    {
                        audiosource.Play();
                        Debug.Log("Success!!!!!!!!!");
                        parentScript.SuccessOrder(3, other.gameObject.GetComponent<Spawned>().owner);
                        StartCoroutine(MoveObj(other.gameObject));
                    } 
                    else
                    {
                        Debug.Log("Wrong Order!!!!!!!!!");
                        parentScript.FailOrder();
                    }
                    break;
            }
        }
    }

    IEnumerator MoveObj(GameObject ToMove)
    {
        float t = 2f;

        ToMove.transform.parent = gameObject.transform;
        Vector3 centerOfServingTray = ToMove.transform.localPosition;
        centerOfServingTray.z = 0f;
        ToMove.transform.localPosition = centerOfServingTray;
        ToMove.transform.localRotation = Quaternion.Euler(0, 0, 0);
        ToMove.GetComponent<Rigidbody>().isKinematic = true;

        Destroy(ToMove.GetComponent<Graspable>());
        foreach (Transform child in ToMove.transform)
        {
            Destroy(child.gameObject.GetComponent<Graspable>());
        }

        while (t >= 0)
        {
            yield return new WaitForSeconds(.1f);
            ToMove.transform.Translate(Time.deltaTime * 1.5f, 0, 0, gameObject.transform);
            t -= 1f*Time.deltaTime;
        }

        ToMove.GetComponent<Spawned>().destroy = true;
        yield break;
    }

    // Returns all children with a given name that are found in the given transform 
    List<Transform> GetChildrenWithName(Transform parent, string name)
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                children.Add(child);
            }
        }

        return children;
    }

    // Start is called before the first frame update
    void Start()
    {
        parentScript = transform.parent.GetComponent<OrderManager>();
    }
}
