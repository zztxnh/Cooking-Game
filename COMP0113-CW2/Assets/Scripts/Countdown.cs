using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class Countdown : MonoBehaviour
{ 
    public TextMeshPro textMesh; // To be set from Inspector

    private float currentTime = 0f;
    private float startingTime = 300f;
    private float speed = 10f;

    void Start()
    {
        currentTime = startingTime;
    }

    void Update()
    {
        currentTime -= 1 *Time.deltaTime;
        string minutes = System.Math.Floor(currentTime / 60).ToString("00");
        string seconds = (currentTime % 60).ToString("00");
        textMesh.text = minutes + ":" + seconds;
        if (currentTime <= 0){
            SceneManager.LoadScene(0);
        }
        transform.Rotate(0, speed * Time.deltaTime, 0, Space.Self);
    }
}
