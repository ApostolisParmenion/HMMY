using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateCamera : MonoBehaviour
{
    public GameObject sceneSun, GameSun;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.RotateAround(Vector3.zero+new Vector3(200,0,0), Vector3.up, 2f * Time.deltaTime);
        //if (GameSun.active)
        //{
        //    sceneSun.SetActive(false);
        //}
        //else
        //{
        //    sceneSun.SetActive(true);

        //}
    }
}
