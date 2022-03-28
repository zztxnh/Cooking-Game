using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeam : MonoBehaviour
{
    public static int TeamNumber;
    public static bool Host;

    // Start is called before the first frame update
    void Start()
    {
        TeamNumber = 1;
        Host = false;
    }

    public void ChangeTeam(int num)
    {
        TeamNumber = num;
    }

    
}
