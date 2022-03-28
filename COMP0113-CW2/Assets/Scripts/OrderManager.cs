using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Generates new orders randomly. Switches panels and controls texts accordingly.
 * Script should be placed in the "Orders Manager" empty gameObject which is the parent of serving tray.
 * Certain GameObjects should be assigned in inspector.
 */
public class OrderManager : MonoBehaviour, INetworkObject, INetworkComponent
{
    // All to be set from inspector
    public GameObject defaultPanel;
    public GameObject currentPanel;
    public GameObject wrongOrderPanel;
    public GameObject correctOrderPanel;
    public List<GameObject> dishesPanels = new List<GameObject>();
    public Text moneyText;
    public Text correctOrderText;

    private int moneyCounter;
    private NetworkContext context;

    public NetworkId Id { get; set; }

    public struct Message
    {
        public int orderNumber;

        public Message(int orderNumber)
        {
            this.orderNumber = orderNumber;
        }
    }

    void INetworkComponent.ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        var msg = message.FromJson<Message>();
        SwitchPanel(dishesPanels[msg.orderNumber]);
    }

    void GenerateRandomOrder(bool owner)
    {
        int randomNumber = Random.Range(0,2);
        //SwitchPanel(dishesPanels[randomNumber]);

        if(owner)
        {
            SwitchPanel(dishesPanels[randomNumber]);
            context.SendJson(new Message(randomNumber));
            //StartCoroutine(SendMsg(randomNumber));
            Debug.Log("Order Sent!!");
        }    

        Debug.Log("New Order Generated");
    }

    void SwitchPanel(GameObject newPanel)
    {
        if (!currentPanel)
        {
            currentPanel = defaultPanel;
        }

        if (currentPanel != newPanel)
        {
            HidePanel(currentPanel);
        }

        ShowPanel(newPanel);
        currentPanel = newPanel;
    }

    public void SuccessOrder(int amount, bool owner) // Used by "ServingTray"
    {
        StartCoroutine(CorrectOrder(amount));
        moneyCounter += amount;
        moneyText.text = moneyCounter.ToString();
        GenerateRandomOrder(owner);
    }

    public void FailOrder() // Used by "ServingTray"
    {
        StartCoroutine(WrongOrder());
    }

    IEnumerator CorrectOrder(int amount)
    {
        correctOrderText.text = "+" + amount.ToString();
        ShowPanel(correctOrderPanel);
        yield return new WaitForSeconds(2);
        HidePanel(correctOrderPanel);
    }

    IEnumerator WrongOrder()
    {
        ShowPanel(wrongOrderPanel);
        yield return new WaitForSeconds(5);
        HidePanel(wrongOrderPanel);
    }

    IEnumerator SendMsg(int num)
    {
        yield return new WaitForSeconds(0.1f);
        context.SendJson(new Message(num));
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

        if (PlayerTeam.Host)
        {
            GenerateRandomOrder(true);
        }
        moneyCounter = 0;
        moneyText.text = moneyCounter.ToString();
    }

    void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartGame()
    {
        GenerateRandomOrder(true);
    }
}
