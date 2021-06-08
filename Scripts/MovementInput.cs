using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MovementInput : MonoBehaviour
{
    public float InputX;
    public float InputZ;
    public Vector3 desiredMoveDirection;
    public float desiredRotationSpeed;
    public bool blockRotationPlayer;
    public Animator anim;
    public float speed;
    public float allowPlayerRotation;
    public CharacterController controller;
    public bool isGrounded;
    public float verticalVel;
    private Vector3 moveVector;
    public float gravity = 10;
    RaycastHit hitInfo;
    public float height = 0.5f;
    public float groundAngle;
    public float maxGroundAngle = 120;
    public float velocity;
    public int jumpVel;
    public float AirTime;
    public AudioSource sound;
    public HealthControl transferDamage;
    public LayerMask ground;
    public Attack fromAttackScript;
    public Collider m_Collider;
    public bool isUnderwater;
    public float waterLevel;
    public bool goFromSwimmingToWalking;
    public Rigidbody rigi;
    public bool isSwimming;
    public bool FlagForRespawn, enterRespawn;

    void Start()
    {
        rigi = this.GetComponent<Rigidbody>();
        anim = this.GetComponent<Animator>();
        controller = this.GetComponent<CharacterController>();
        sound = this.GetComponent<AudioSource>();
        jumpVel = 0;
        desiredMoveDirection = Vector3.forward;
        fromAttackScript.isAttacking = false;
        GetComponent<Player>().deadSoReturn = false;
        isSwimming = false;
        FlagForRespawn = false;
        enterRespawn = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (GetComponent<Player>().deadSoReturn)
        {
            if (transferDamage.dead && FlagForRespawn == false && enterRespawn)
            {
                StartCoroutine(WaitForRespawn());
                WaitForRespawn();
                StopCoroutine(WaitForRespawn());
                enterRespawn = false;
            }
            else if (FlagForRespawn == true && transferDamage.respawned==false)
            {
                GetComponent<Player>().deadSoReturn = false;
                anim.SetBool("dead", false);
                rigi.useGravity = false;
                rigi.constraints = RigidbodyConstraints.FreezeAll;
                controller.enabled = false;
                controller.transform.position = new Vector3(284.69f, 241.17f, 267.42f);
                controller.enabled = true;
                enterRespawn = true;
                FlagForRespawn = false;
                transferDamage.respawned = true;
            }
            return;
        }
        else if (transferDamage.dead)
        {
            // rigi.useGravity = true;
            rigi.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
            anim.SetBool("dead", true);
            anim.Play("Death");
            GetComponent<Player>().deadSoReturn = true;

            return;
        }

        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        anim.SetFloat("InputZ", InputZ, 0.0f, Time.deltaTime * 2f);
        anim.SetFloat("InputX", InputX, 0.0f, Time.deltaTime * 2f);

        if (isSwimming)
        {
            isUnderwater = true;
            goFromSwimmingToWalking = true;
            
        }
        else
        {
            isUnderwater = false;
            goFromSwimmingToWalking = false;
        }
        if (isUnderwater)
        {
            print("edashjiuohdiuashiujo");
            AirTime = 0;
            if (verticalVel < 0)
            {
                verticalVel += 0.02f;
                moveVector = new Vector3(0, verticalVel, 0);
                controller.Move(moveVector);
            }
            if (Input.GetButton("Jump"))
            {
                controller.Move(new Vector3(0, 0.08f, 0));
            }
            else if (Input.GetButton("left shift"))
            {
                controller.Move(new Vector3(0, -0.08f, 0));
            }
            else
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            CancelAnimations("swimming");
            anim.Play("swimming");
            return;
        }

        if (!fromAttackScript.isAttacking)
        {
            inputMagnitude();
        }


        if (desiredMoveDirection == new Vector3(1, 0, 1))
        {
            desiredMoveDirection = transform.forward;
        }
        if (Physics.Raycast(transform.position + Vector3.up + 0.05f * desiredMoveDirection, Vector3.down, out hitInfo, 1.8f, ground) && verticalVel < 0)
        {
            isGrounded = true;
            if (AirTime > 45)
            {
                transferDamage.fallFlag = true;
                transferDamage.damageFromFalling = AirTime;
            }

            AirTime = 0;
        }
        else
        {
            isGrounded = false;
            AirTime += 0.5f;
        }

        if (isGrounded)
        {
  //          if (goFromSwimmingToWalking && transform.position.x>waterLevel+1)
  //          {
  //              controller.Move(transform.forward+new Vector3(0,1,0));
  //              anim.Play("jump");
  //              goFromSwimmingToWalking = false;
  //          }
            verticalVel = -0.03f;

        }
        else
        {
            verticalVel -= 0.04f * (gravity * Time.deltaTime);
        }
        moveVector = new Vector3(0, verticalVel, 0);
        controller.Move(moveVector);
        //print(verticalVel);

    }

    void playerMoveAndrotation()
    {

        var forward = transform.forward;
        var right = transform.right;

        forward.y = 0f;
        right.y = 0f;

       // forward.Normalize();
        //right.Normalize();
        desiredMoveDirection = forward * InputZ + right  * InputX;

        groundAngle = Vector3.Angle(hitInfo.normal, desiredMoveDirection);

        if (groundAngle < maxGroundAngle)
        {
            controller.Move(velocity * (new Vector3(desiredMoveDirection.x, -0.1f, desiredMoveDirection.z)));
        }

    }

    void inputMagnitude()
    {

        speed = new Vector2(InputX, InputZ).sqrMagnitude;

        if (verticalVel < 0)
        {
            if (isGrounded)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    Jump();
                }
                else if (Input.GetButton("left shift") && (InputZ == 1))
                {
                    Running();
                }
                else
                {
                    Walking();
                }
            }
            else
            {
                if (AirTime > 30)
                {
                    Falling();
                }
            }
        }


        if (speed > allowPlayerRotation)
        {
            anim.SetFloat("inputMagnitude", speed, 0.0f, Time.deltaTime);
            playerMoveAndrotation();
        }
        else if (speed < allowPlayerRotation)
        {
            anim.SetFloat("inputMagnitude", speed, 0.0f, Time.deltaTime);
        }


    }
    void Walking()
    {
        CancelAnimations("walking");
        velocity = 0.02f;

    }
    void Running()
    {
        CancelAnimations("running");
        velocity = 0.05f;
    }

    void Jump()
    {
        anim.Play("jump");
       // sound.Play();
        verticalVel = 0.12f;
    }
    void Falling()
    {
        
        CancelAnimations("falling");
    }
    void CancelAnimations(string str)
    {
        anim.SetBool("falling", false);
        anim.SetBool("jumping", false);
        anim.SetBool("running", false);
        anim.SetBool("walking", false);
        anim.SetBool("swimming", false);
        anim.SetBool(str, true);
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "LavaCube")
        {
            transferDamage.burned = true;
        }
        if (col.gameObject.name == "SwimmingCollider")
        {
            isSwimming = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "SwimmingCollider")
        {
            isSwimming = false;
            CancelAnimations("walking");
        }
    }

        IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(5);
        FlagForRespawn = true;
    }
}

 

