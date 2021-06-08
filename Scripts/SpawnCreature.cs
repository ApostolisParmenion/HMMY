using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCreature : NetworkBehaviour
{
    public GameObject Animal,Creature;
    public bool spawnNext, isAnimal, timeSet, distanceOK;
    public float whenToSpawn;
    public float spawnDelay=0;
    public SunRotation checkIfNight;
    public List<GameObject> animals;
    GameObject creaturesName;
    GameObject[] players;
    float temp,min;
    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            spawnNext = true;
            InvokeRepeating("Spawn", 1f, 1f);
            timeSet = false;
            checkIfNight = GameObject.Find("Sun").GetComponent<SunRotation>();
            isAnimal = true;
            creaturesName=null;
        }
    }
    void Spawn()
    {
        {
            players = GameManager.playersObjects;

            try
            {
                min = 9999;
                foreach (var tempPlayer in players)
                {
                    if (tempPlayer != null)
                    {
                        temp = (tempPlayer.transform.position - transform.position).magnitude;
                        if (temp < min)
                        {
                            min = temp;
                        }
                    }

                }
            }
            catch
            {
                return;
            }
            if(Mathf.Abs(min) < 150)
            {
                distanceOK = true;
            }
            else
            {
                distanceOK = false;
            }
            if (spawnNext && !timeSet)
            {
                whenToSpawn = Time.time + spawnDelay;
                timeSet = true;
            }

            if (!isAnimal)
            {
                if (checkIfNight.isNight)
                {

                    if (Time.time > whenToSpawn && timeSet && distanceOK)
                    {
                        CmdSpawnCreature("Creature");
                        spawnNext = false;
                        timeSet = false;
                    }
                    else if (!distanceOK)
                    {
                        if(creaturesName!=null && creaturesName.GetComponent<MonsterMovement>().Health>90)
                        {
                            CmdDestroy();
                            spawnNext = true;
                            isAnimal = false;
                        }
                    }

                }
                else if (!checkIfNight.isNight)
                {
                    isAnimal = true;
                    spawnNext = true;
                    if (creaturesName != null)
                    {
                        CmdDestroy();
                    }
                }
            }
            else
            {
                if (!checkIfNight.isNight)
                {

                    if (Time.time > whenToSpawn && timeSet && distanceOK)
                    {
                        CmdSpawnCreature("Animal");
                        spawnNext = false;
                        timeSet = false;
                    }
                    else if (!distanceOK)
                    {
                        if (creaturesName != null && creaturesName.GetComponent<AnimalMovement>().Health > 90)
                        {
                            CmdDestroy();
                            spawnNext = true;
                            isAnimal = false;
                        }

                    }

                }
                else if(checkIfNight.isNight)
                {
                    spawnNext = true;
                    isAnimal = false;
                    if (creaturesName != null)
                    {
                        CmdDestroy();
                    }
                }
            }
        }
    }
    [Command]
    void CmdSpawnCreature(string type)
    {
        if (type == "Creature")
        {
            creaturesName = (GameObject)Instantiate(Resources.Load(Creature.name, typeof(GameObject)), transform.position, transform.rotation,transform);
            creaturesName.name = Creature.name;
            NetworkServer.Spawn(creaturesName);
        }
        else
        {
            Animal = animals[Random.Range(0, animals.Count)];
            creaturesName = (GameObject)Instantiate(Resources.Load(Animal.name, typeof(GameObject)), transform.position, transform.rotation,transform);
            creaturesName.name = Animal.name;
            NetworkServer.Spawn(creaturesName);
        }
    }

    [Command]

    void CmdDestroy()
    {
        if (creaturesName != null)
        {
            Destroy(creaturesName);
        }
        creaturesName=null;
    }

    }
