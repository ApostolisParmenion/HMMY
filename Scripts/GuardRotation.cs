using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardRotation : MonoBehaviour {
    public CameraMovement objectWithCameraRotation;
    public HealthControl isdead;
    public Vector3 position;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        position = transform.position;

        if (!isdead.dead)
        {
            if (!Input.GetButton("left alt"))
            {
                transform.eulerAngles = new Vector3(0, objectWithCameraRotation.right_left, 0);
            }
        }
       

    }
}
