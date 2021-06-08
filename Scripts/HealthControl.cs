using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthControl : MonoBehaviour
{
    public RectTransform control;
    public float damageFromFalling;
    public float damageFromAttack;
    public bool fallFlag;
    public bool attFlag;
    public Text Health;
    public Text Armor;
    public Text Damage;
    public string Name;
    public bool dead;
    public bool burned;
    public bool eaten;
    public float eatHeal;
    public bool respawned;
    public int armor;

    // Start is called before the first frame update
    void Start()
    {
        control = this.GetComponent<RectTransform>();
        control.localPosition = new Vector3(0, 0, 0);
        damageFromFalling = 0;
        fallFlag = false;
        attFlag = false;
        Health.text = "100";
        dead = false;
        eaten = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fallFlag == true)
        {
            damageFromFalling = 1.5f * damageFromFalling;
            print("Received " + damageFromFalling + " falling damage");
            control.localPosition += new Vector3(-damageFromFalling, 0, 0);
            Health.text = "" + (float.Parse(Health.text) - damageFromFalling);
            fallFlag = false;
        }
        if (attFlag == true)
        {
            control.localPosition += new Vector3(-damageFromAttack * (100 - armor) / 100, 0, 0);
            attFlag = false;
            Health.text = "" + (float.Parse(Health.text) - damageFromAttack * (100 - armor) / 100);
        }
        if (respawned || float.Parse(Health.text) > 100)
        {
            control.localPosition = new Vector3(0, 0, 0);
            Health.text = "100";
            respawned = false;
            dead = false;
        }
        if (burned == true)
        {
            print("you have fallen into lava");
            control.localPosition = new Vector3(-100, 0, 0);
            Health.text = "0";
            burned = false;
        }
        if (float.Parse(Health.text) <= 0)
        {
            Health.text = "0";
            dead = true;
        }
        else
        {
            dead = false;
        }
        if (eaten)
        {
            eaten = false;
            if (float.Parse(Health.text) < 100)
            {
                control.localPosition += new Vector3(eatHeal, 0, 0);
                Health.text = "" + (float.Parse(Health.text) + eatHeal);
                eaten = false;
            }
        }
    }
}
