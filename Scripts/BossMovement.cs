using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BossMovement : MonsterMovement
{
    bool addedtolist;
    public LoadScene GameManagerLoadScene;
    private void Start()
    {
        addedtolist = false;
        base.Start();
    }
    void Update()
    {
        base.Update();
        if (isServer && dead && !addedtolist)
        {
            GameManagerLoadScene.AddToList(gameObject.name);
            addedtolist = true;
        }
    }
}
