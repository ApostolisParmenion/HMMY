using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float right_left, upNdown, temp;
    public GameObject Guard;
    public Camera cam;

    // Update is called once per frame

    private void Start()
    {
        cam.tag = "MainCamera";
    }
    void Update()
    {
        //transform.position = Guard.transform.position + 2f * Vector3.up;
        right_left += Input.GetAxis("Mouse X") * 3;
        upNdown += Input.GetAxis("Mouse Y") * 2;
        if (upNdown > -50 && upNdown < 50)
        {
            transform.eulerAngles = new Vector3(upNdown, right_left, 0);
            temp = upNdown;
        }
        else
        {
            transform.eulerAngles = new Vector3(temp, right_left, 0);
            upNdown = temp;
        }
    }
}
