using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamsPanel : MonoBehaviour, INetworkObject, INetworkComponent
{
    public GameObject Image1; // To be set from Inspector
    public GameObject Image2; // To be set from Inspector
    public GameObject Tutorial; // To be set from Inspector

    public NetworkId Id { get; set; }

    private NetworkContext context;

    public struct Message
    {
        public bool started;

        public Message(bool started)
        {
            this.started = started;
        }
    }

    void INetworkComponent.ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        StartCoroutine(StartGame());
    }

    public void ChangeImage(int num)
    {
        if (num == 1)
        {
            Image1.SetActive(true);
            Image2.SetActive(false);
        }
        else if (num == 2)
        {
            Image2.SetActive(true);
            Image1.SetActive(false);
        }
    }


    public void StartTutorial()
    {
        context.SendJson(new Message(true));
        PlayerTeam.Host = true;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        Tutorial.SetActive(true);
        yield return new WaitForSeconds(83);
        SceneManager.LoadScene(1);
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
    }
}
