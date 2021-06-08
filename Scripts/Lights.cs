using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour
{
    public bool flag;
    public SunRotation sunScript;
    GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        flag = true;
        InvokeRepeating("OpenLights", 5f, 2f);
    }

    // Update is called once per frame
    void OpenLights()
    {
        players = GameManager.playersObjects;

        if (!sunScript.isNight)
        {
            if (flag == false)
            {
                this.GetComponent<Light>().enabled = false;
                flag = true;
            }

        }
        else
        {
            players = GameManager.playersObjects;
            foreach (var tempPlayer in players)
            {
                if ((transform.position - tempPlayer.transform.position).magnitude < 150)
                {
                    this.GetComponent<Light>().enabled = true;
                    flag = false;
                    return;
                }
            }
            if (flag)
            {
                this.GetComponent<Light>().enabled = false;
                flag = true;
            }
        }
    }
}
