using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExpControl : MonoBehaviour
{
    public RectTransform control;
    public Text Level, percentage;
    public bool gotExp;
    public float expGained;
    public int expRequired;
    public float positionMove;
    public float expAcquired;
    public bool levelUp;
    public HealthControl transferArmor;

    // Start is called before the first frame update
    void Start()
    {
        control = this.GetComponent<RectTransform>();
        control.localPosition = new Vector3(-100, 0, 0);
        Level.text = "1";
        percentage.text = "0";
        gotExp = false;
        expRequired = 100;
        expAcquired = 0;
        transferArmor.armor = 7;
        transferArmor.Armor.text = "" + transferArmor.armor;
    }

    // Update is called once per frame
    void Update()
    {
        if (float.Parse(Level.text) >= 11)
        {
            return;
        }
        if (float.Parse(percentage.text) >= 100)
        {
            transferArmor.armor += 9;
            transferArmor.Armor.text = "" + transferArmor.armor;
            Level.text = "" + (float.Parse(Level.text) + 1);
            expRequired *= 2;
            percentage.text = "0";
            control.localPosition = new Vector3(-100, 0, 0);
            if (expGained > 0 && levelUp == true)
            {
                print(expGained);
                levelUp = false;
                expAcquired = expGained;
                positionMove = expGained / expRequired * 100;
                control.localPosition += new Vector3(positionMove, 0, 0);
                percentage.text = "" + (float.Parse(percentage.text) + positionMove).ToString("0.00");
            }
        }
        if (gotExp)
        {
            expAcquired += expGained;
            gotExp = false;
            positionMove = expGained / expRequired * 100;
            control.localPosition += new Vector3(positionMove, 0, 0);
            percentage.text = "" + (float.Parse(percentage.text) + positionMove).ToString("0.00");
            if (expRequired < expGained + expAcquired)
            {
                levelUp = true;
                expGained = expAcquired - expRequired;
            }
        }

    }
}
