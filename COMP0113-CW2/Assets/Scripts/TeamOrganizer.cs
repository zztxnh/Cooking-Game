using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamOrganizer : MonoBehaviour
{
    //public TeamManager teamScript;
    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.Find("Player");
        //PlayerTeam teamScript = player.GetComponent<PlayerTeam>();
        
        if(PlayerTeam.TeamNumber == 1)
        {
            player.transform.position = new Vector3(-0.5f, 2.32f, -0.5f);
            player.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            player.transform.position = new Vector3(-0.5f, 2.32f, 5f);
            player.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        
    }

}
