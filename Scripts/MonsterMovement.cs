using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.AI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System;

public class MonsterMovement : NetworkBehaviour
{

    public Vector3 startingPos;
    public float maxX = 1;
    public float maxZ = 1;
    Quaternion rotation;
    public Animator anim;
    public CharacterController controler;
    public Vector3 desiredPosition;
    public Vector3 distance;
    public bool isMoving;
    public bool flag;
    public Vector3 distanceFromDestination;
    public Vector3 distanceFromCenter;
    public float minDistance;
    public float maxCenterDistance;
    Vector3 startingCenter;
    public int waitTime;
    public bool spotted;
    public Vector3 targetLocation;
    public Vector3 targetDistance;
    public float maxTargetDistance;
    public Collider m_Collider;
    public CapsuleCollider s_collider;
    public bool DeadSoReturn;
    public bool hit, attacked;
    public Rigidbody swma;
    public NavMeshAgent EnemyAgent;
    public bool doNothing;
    public int despawnDelay;
    public int experience;
    [SyncVar] public float Health;
    public RectTransform control, CanvasLookAt;
    [SyncVar] public bool dead;
    [SyncVar] public float armor;
    [SyncVar] public bool ExpGiven;
    public float temp;
    GameObject[] players;
    GameObject closestPlayer, MainCamera;
    float min;
    public void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (MainCamera.gameObject.transform.root.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            MainCamera = null;
        }
        if (isServer)
        {
            players = null;
            ExpGiven = false;
            despawnDelay = 20;
            EnemyAgent = this.GetComponent<NavMeshAgent>();
            flag = true;
            startingCenter = transform.position;
            anim = this.GetComponent<Animator>();
            controler = this.GetComponent<CharacterController>();
            isMoving = false;
            distanceFromDestination = Vector3.zero;
            spotted = false;
            minDistance = 1;
            m_Collider = this.GetComponent<Collider>();
            s_collider = this.GetComponent<CapsuleCollider>();
            swma = this.GetComponent<Rigidbody>();
            doNothing = false;
            DeadSoReturn = false;
            Health = 100;
            dead = false;
            if (armor < 5)
            {
                armor = 5;
            }
            control.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            control.localPosition = new Vector3(Health - 100, 0, 0);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (isServer)
        {
            try
            {
                if (GameManager.playersObjects.Length > 0)
                    players = GameManager.playersObjects;
            }
            catch
            {
                return;
            }

            min = 9999;

            if (DeadSoReturn)
            {
                return;
            }
            if (dead)
            {
                anim.SetBool("dead", true);
                anim.Play("Death");
                m_Collider.enabled = !m_Collider.enabled;
                s_collider.enabled = !s_collider.enabled;
                DeadSoReturn = true;

                StartCoroutine(ChangeDirection());
                ChangeDirection();
                StopCoroutine(ChangeDirection());
                return;
            }
            controler.Move(new Vector3(0, -1, 0));
            closestPlayer = null;
            foreach (var tempPlayer in players)
            {
                if (tempPlayer != null)
                {
                    targetLocation = tempPlayer.transform.position;
                    targetDistance = transform.position - targetLocation;
                    if (targetDistance.magnitude < min && !tempPlayer.GetComponent<Player>().deadSoReturn)
                    {
                        min = targetDistance.magnitude;
                        closestPlayer = tempPlayer;
                    }
                }
            }
            if (closestPlayer != null)
            {
                targetLocation = closestPlayer.transform.position;
                targetDistance = transform.position - targetLocation;
            }
            else
            {
                targetDistance = transform.position + transform.position;
            }
            //health = targetHealth.Health;
            if (targetDistance.magnitude < maxTargetDistance)// && float.Parse(health.text)>0)
            {
                spotted = true;
                anim.SetBool("targetSpoted", true);
            }
            else
            {
                spotted = false;
                anim.SetBool("targetSpoted", false);
            }
            Debug.DrawLine(startingCenter, (startingCenter + 5 * Vector3.up));
            Debug.DrawLine(transform.position, desiredPosition);

            if (spotted)
            {
                EnemyAgent.speed = 0;
                isMoving = false;
                flag = true;
                anim.SetBool("isMoving", false);
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
                {
                    desiredPosition = closestPlayer.transform.position - new Vector3(0, 0.6f, 0);
                    distance = new Vector3(desiredPosition.x - transform.position.x, desiredPosition.y - transform.position.y + 0.4f, desiredPosition.z - transform.position.z);
                    rotation = Quaternion.LookRotation(distance);
                    transform.rotation = rotation;
                    EnemyAgent.SetDestination(desiredPosition);
                }
                if (distance.magnitude < 2.5f)
                {
                    anim.SetBool("isAttacking", true);
                    swma.AddForceAtPosition(Vector3.one, desiredPosition);

                }
                else
                {

                    anim.SetBool("isAttacking", false);

                }

            }
            else
            {
                anim.SetBool("isAttacking", false);
                if (!isMoving && flag)
                {
                    EnemyAgent.speed = 0;
                    flag = false;
                    waitTime = UnityEngine.Random.Range(3, 10);

                    startingPos = transform.position;
                    desiredPosition = new Vector3(UnityEngine.Random.Range(transform.position.x, transform.position.x + maxX) + UnityEngine.Random.Range(0, -maxX), transform.position.y, UnityEngine.Random.Range(transform.position.z, transform.position.z + maxZ) + UnityEngine.Random.Range(0, -maxZ));

                    distanceFromCenter = startingCenter - transform.position;
                    if (distanceFromCenter.magnitude > maxCenterDistance)
                    {
                        desiredPosition = startingCenter;
                    }



                    doNothing = true;
                    StartCoroutine(ChangeDirection());
                    ChangeDirection();
                    StopCoroutine(ChangeDirection());

                }
                else if (doNothing == false)
                {
                    isMoving = true;
                    flag = false;
                    anim.SetBool("isMoving", true);
                    distanceFromDestination = desiredPosition - transform.position;
                    distanceFromDestination.y = 0;
                    EnemyAgent.speed = 4;
                    EnemyAgent.SetDestination(desiredPosition);
                    if (distanceFromDestination.magnitude < minDistance)
                        if (Mathf.Abs(distanceFromDestination.x) < minDistance && Mathf.Abs(distanceFromDestination.z) < minDistance)
                        {
                            isMoving = false;
                            flag = true;
                            anim.SetBool("isMoving", false);
                        }

                }
            }
        }
        try
        {
            CanvasLookAt.transform.LookAt(MainCamera.transform);
        }
        catch
        {
            MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            if (MainCamera.gameObject.transform.root.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                MainCamera = null;
            }
        }
        if (temp != Health)
        {
            control.localPosition = new Vector3(Health - 100, 0, 0);
            temp = Health;
        }

    }

    public void changeHealth(float damage)
    {
        Health += -damage * (100 - armor) / 100;
        if (Health <= 0)
        {
            dead = true;
        }
    }
    [Command]
    void CmdDestroyObject()
    {
        Destroy(gameObject);
    }
    IEnumerator ChangeDirection()
    {

        EnemyAgent.speed = 0;
        if (!DeadSoReturn)
        {
            yield return new WaitForSeconds(waitTime);
            isMoving = true;
            anim.SetBool("isMoving", true);
            EnemyAgent.SetDestination(desiredPosition);
            doNothing = false;
        }
        else
        {
            yield return new WaitForSeconds(despawnDelay);
            CmdDestroyObject();
        }
    }


    public void OnTriggerEnter(Collider col)
    {
        if (isServer)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Weapon") &&
                col.gameObject.transform.root.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack") &&
                !col.GetComponent<WeaponDamage>().checkIfAlreadyAttacked)
            {
                col.GetComponent<WeaponDamage>().checkIfAlreadyAttacked = true;
                print(gameObject.name + " attacked by " + col.gameObject.transform.root.name);
                changeHealth(col.GetComponent<WeaponDamage>().damage);
                if (dead && !ExpGiven)
                {
                    GameObject[] PlayersToGetExp = new GameObject[players.Length];
                    int i = 0;
                    foreach (var tempPlayer in players)
                    {
                        if ((tempPlayer.transform.position - transform.position).magnitude < 50)
                        {
                            PlayersToGetExp[i] = tempPlayer;
                            i++;
                        }
                    }
                    RpcUpdateExpOnAllClients(PlayersToGetExp, i);
                    ExpGiven = true;
                    string[] names = new string[i];
                    for (int j = 0; j < i; j++)
                    {
                        if (!PlayersToGetExp[j].GetComponent<PlayerSetup>().isserver)
                        {
                            PlayersToGetExp[j].GetComponentInChildren<ExpControl>().expGained = (experience / i) + (experience * Mathf.Log10(i));
                            PlayersToGetExp[j].GetComponentInChildren<ExpControl>().gotExp = true;
                        }
                        names[j] = PlayersToGetExp[j].GetComponent<PlayerSetup>().USERNAME;
                    }
                    CmdUpdateExpDatabase(names, experience);
                }
            }
        }
    }

    [ClientRpc]
    public void RpcUpdateExpOnAllClients(GameObject[] PlayersToGetExp, int i)
    {

        for (int j = 0; j < i; j++)
        {
            PlayersToGetExp[j].GetComponentInChildren<ExpControl>().expGained = (experience / i) + (experience * Mathf.Log10(i));
            PlayersToGetExp[j].GetComponentInChildren<ExpControl>().gotExp = true;
        }
    }

    [Command]
    public void CmdUpdateExpDatabase(string[] Username, float expGained)
    {
        FileStream file = File.Open("Accounts.txt", FileMode.OpenOrCreate, FileAccess.Read);
        List<PlayerInfo> AllAccounts = new List<PlayerInfo>();
        try
        {
            BinaryFormatter b = new BinaryFormatter();
            try
            {
                AllAccounts = (List<PlayerInfo>)b.Deserialize(file);
            }
            catch
            {
                print("Failed to update exp");
            }
            file.Close();
            for (int i = 0; i < AllAccounts.Count; i++)
            {
                foreach (var tempusername in Username)
                {
                    if (AllAccounts[i].Username == tempusername)
                    {
                        AllAccounts[i].TotalExp += expGained;
                    }
                }
            }
            file = File.Open("Accounts.txt", FileMode.OpenOrCreate, FileAccess.Write);
            b.Serialize(file, AllAccounts);
            file.Close();
        }
        catch (Exception ex)
        {
            print(ex.Message);
            file.Close();
        }
    }
}

