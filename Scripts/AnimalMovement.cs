using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class AnimalMovement : NetworkBehaviour
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
    public Vector3 distanceFromDestination;
    public Vector3 distanceFromCenter;
    public float minDistance;
    public float maxCenterDistance;
    Vector3 startingCenter;
    public int waitTime;
    public MeshCollider m_collider;
    public bool DeadSoReturn;
    public bool attacked;
    public Rigidbody swma;
    public NavMeshAgent navAgent;
    public bool doNothing;
    public SpawnCreature parentsBool;
    public int despawnDelay;
    public bool flag;
    public int howMuchFoodToDrop;
    public GameObject food, MainCamera;
    GameObject foodObj;
    [SyncVar]
    public float Health;
    public RectTransform control, CanvasLookAt;
    public bool dead;
    public float armor,speed=2;
    float temp;
    void Start()
    {
        temp = 100f;
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (MainCamera.gameObject.transform.root.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            MainCamera = null;
        }
        if (isServer)
        {
            anim = this.GetComponent<Animator>();
            controler = this.GetComponent<CharacterController>();
            parentsBool = GetComponentInParent<SpawnCreature>();
            m_collider = this.GetComponent<MeshCollider>();
            navAgent = this.GetComponent<NavMeshAgent>();
            swma = this.GetComponent<Rigidbody>();
            despawnDelay = 20;
            startingCenter = transform.position;
            isMoving = false;
            distanceFromDestination = Vector3.zero;
            minDistance = 1;
            doNothing = false;
            DeadSoReturn = false;
            flag = true;
            Health = 100;
            dead = false;
            if (armor < 4)
            {
                armor = 4;
            }
            control.localPosition = new Vector3(0, 0, 0);

        }
        else
        {
            control.localPosition = new Vector3((Health - 100), 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            if (Health <= 0)
            {
                Health = 0;
                dead = true;
            }
            if (DeadSoReturn)
            {
                return;
            }
            if (dead)
            {
                anim.Play("Death");
                m_collider.enabled = !m_collider.enabled;
                DeadSoReturn = true;

                StartCoroutine(ChangeDirection());
                ChangeDirection();
                StopCoroutine(ChangeDirection());
                CmdSpawnFood();
                return;
            }
            controler.Move(new Vector3(0, -1, 0));
            //Debug.DrawLine(startingCenter, (startingCenter + 5 * Vector3.up));
            //Debug.DrawLine(transform.position, desiredPosition);
            if (!isMoving && flag)
            {
                flag = false;
                navAgent.speed = 0;
                waitTime = Random.Range(3,8);
                startingPos = transform.position;
                desiredPosition = new Vector3(Random.Range(transform.position.x, transform.position.x + maxX) + Random.Range(0, -maxX), transform.position.y, Random.Range(transform.position.z, transform.position.z + maxZ) + Random.Range(0, -maxZ));

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
                anim.SetBool("isMoving", true);
                anim.Play("walk");
                distanceFromDestination = desiredPosition - transform.position;
                distanceFromDestination.y = 0;
                navAgent.speed = speed;
                navAgent.SetDestination(desiredPosition);
                if (distanceFromDestination.magnitude < minDistance)
                {
                    if (Mathf.Abs(distanceFromDestination.x) < minDistance && Mathf.Abs(distanceFromDestination.z) < minDistance)
                    {
                        isMoving = false;
                        anim.SetBool("isMoving", false);
                        anim.Play("idle");
                        flag = true;
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
            control.localPosition = new Vector3((Health - 100), 0, 0);
            temp = Health;
        }

    }

    public void changeHealth(float damage)
    {
        Health += -damage * (100 - armor) / 100;
    }


    [Command]
    void CmdSpawnFood()
    {
        for (int i = 0; i < Random.Range(1, howMuchFoodToDrop); i++)
        {
            foodObj = (GameObject)Instantiate(Resources.Load(food.name, typeof(GameObject)), transform.position + 2 * Vector3.up + 2 * Vector3.forward, transform.rotation);
            foodObj.name = food.name;
            NetworkServer.Spawn(foodObj);
        }
    }

    [Command]
    void CmdDestroyObject()
    {
        Destroy(gameObject);
    }
    IEnumerator ChangeDirection()
    {

        navAgent.speed = 0;
        if (!DeadSoReturn)
        {
            yield return new WaitForSeconds(waitTime);
            isMoving = true;
            anim.SetBool("isMoving", true);
            navAgent.SetDestination(desiredPosition);
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
            GameObject root = col.gameObject.transform.root.gameObject;
            if (col.gameObject.layer == LayerMask.NameToLayer("Weapon") && root.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack") && !col.GetComponent<WeaponDamage>().checkIfAlreadyAttacked)
            {
                col.GetComponent<WeaponDamage>().checkIfAlreadyAttacked = true;
                print("Attacked by " + col.gameObject.transform.root.name);
                changeHealth(col.GetComponent<WeaponDamage>().damage);
            }
        }
    }
}

