using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class removeWall : MonoBehaviour
{
    public BossMovement BossIsDead;
    bool addedtolist;
    public LoadScene GameManagerLoadScene;

    void Update()
    {
        if (BossIsDead.dead == true)
        {
            Destroy(gameObject);
            if (BossIsDead.isServer && !addedtolist)
            {
                GameManagerLoadScene.AddToList(gameObject.name);
                addedtolist = true;
            }
        }
    }
}
