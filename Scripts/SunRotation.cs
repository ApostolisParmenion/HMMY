using UnityEngine.Networking;

using UnityEngine;

public class SunRotation : NetworkBehaviour
{
    public Light fws;
    public bool isNight;
    public float speed=0.1f;
    public GameObject sunny,noonMorning,night,postProNight;
    public string SkyObject;
    void Start()
    {
        fws = this.GetComponent<Light>();
        fws.enabled = true;
        InvokeRepeating("MoveSun", 0f, 0.05f);
        SkyObject = "sunny";
        isNight = false;
        noonMorning.SetActive(false);
        night.SetActive(false);
    }

    // Update is called once per frame
    void MoveSun()
    {
        if (SkyObject == "sunny")
        {
            if (transform.position.y < 900)
            {
                sunny.SetActive(false);
                noonMorning.SetActive(true);
                night.SetActive(false);
                postProNight.SetActive(false);
                SkyObject = "NoonMorning";
            }
        }
        else if (SkyObject == "NoonMorning")
        {
            if (transform.position.y > 900)
            {
                sunny.SetActive(true);
                noonMorning.SetActive(false);
                night.SetActive(false);
                postProNight.SetActive(false);
                SkyObject = "sunny";
            }
            else if(transform.position.y < -200){
                SkyObject = "Night";
                sunny.SetActive(false);
                noonMorning.SetActive(false);
                night.SetActive(true);
                postProNight.SetActive(true);
            }
        }
        else if (SkyObject == "Night")
        {
            if (transform.position.y > -200)
            {
                sunny.SetActive(false);
                noonMorning.SetActive(true);
                night.SetActive(false);
                postProNight.SetActive(false);
                SkyObject = "NoonMorning";
            }
        }
        if (isServer)
        {
            transform.RotateAround(Vector3.zero, Vector3.forward, speed * Time.deltaTime);
        }
        if (transform.position.y > -200)
        {
            if(transform.position.y >0)
                fws.colorTemperature = transform.position.y / 0.3f;
            if (isNight)
            {
                fws.enabled = true;
                isNight = false;
            }
        }
        else
        {
            fws.colorTemperature = 2000;
            if (!isNight)
            {
                isNight = true;
                fws.enabled = false;
            }
        }
    }
}
