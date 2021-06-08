using UnityEngine;

public class rotateHealth : MonoBehaviour
{
    Transform Camera;
    //Set it to whatever value you think is best
    private void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        InvokeRepeating("UpdateCamera",0,4);
    }
    void Update()
    {
        transform.LookAt(Camera);
        transform.Rotate(0, 180, 0);
    }
    void UpdateCamera()
    {
        if (Camera.transform.root.gameObject != transform.root.gameObject)
        {
            Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
            return;
        }
    }
}
