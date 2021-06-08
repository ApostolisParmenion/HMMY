using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public HealthControl isdead;
    public bool isAttacking;
    public Animator anim;
    public MovementInput movementInputScript;
    public Inventory Inventory;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isAttacking = false;
        Inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movementInputScript.isUnderwater || isdead.dead || !Inventory.canAttack)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1") && isAttacking == false)
        {
            isAttacking = true;
            anim.SetBool("attacking", true);
            anim.SetBool("running", false);
            anim.Play("attack");
        }
        else if(!anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            
            isAttacking = false;
            anim.SetBool("attacking", false);
        }
    }
}
