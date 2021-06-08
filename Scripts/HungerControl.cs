using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HungerControl : MonoBehaviour
{
    public RectTransform control;
    public HealthControl healthScript;
    public Text Hunger;
    public bool eaten;
    public float hungerHeal;
    public bool hungry;


    // Start is called before the first frame update
    void Start()
    {
        control = this.GetComponent<RectTransform>();
        control.localPosition = new Vector3 (0,0,0);
        Hunger.text = "100";
        eaten = false;
        hungry = false;

        InvokeRepeating("HungerIncreasment", 200f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (float.Parse(Hunger.text) <= 0)
        {
            Hunger.text = "0";
            hungry = true;
        }
        else
        {
            hungry = false;
        }
        if (float.Parse(Hunger.text) >= 100)
        {
            Hunger.text = "100";
            control.localPosition += new Vector3(0, 0, 0);
        }
        if (eaten)
        {
            eaten = false;
            if (float.Parse(Hunger.text) < 100) {
                control.localPosition += new Vector3(hungerHeal, 0, 0);
                Hunger.text = "" + (float.Parse(Hunger.text) + hungerHeal);
            }
        }
    }

    void HungerIncreasment()
    {
        if (!hungry)
        {
            control.localPosition -= new Vector3(1, 0, 0);
            Hunger.text = "" + (float.Parse(Hunger.text) - 1);
        }
        else if (!healthScript.dead)
        {
            healthScript.attFlag = true;
            healthScript.damageFromAttack = 1;
            healthScript.Name = "Hunger";
        }
        if (healthScript.dead)
        {
            Hunger.text = "50";
            control.localPosition = new Vector3(-50, 0, 0);
        }
    }
}
