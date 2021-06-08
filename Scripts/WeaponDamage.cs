using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDamage : MonoBehaviour
{
    public int damage;
    public Player player;
    public Rigidbody rigi;
    public BoxCollider col;
    public bool WorldObject;
    public Animator PlayerAnimator;
    public bool checkIfAlreadyAttacked; // false ama den exw varesei
    // Start is called before the first frame update
    void Start()
    {
        rigi = this.GetComponent<Rigidbody>();
        col = this.GetComponent<BoxCollider>();
        GiveValue();
        PlayerAnimator = transform.root.GetComponent<Animator>();
        checkIfAlreadyAttacked = false;
    }
    private void GiveValue()
    {
        col.isTrigger = true;
    }

    private void Update()
    {
        if (!PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack") && checkIfAlreadyAttacked)
        {
            checkIfAlreadyAttacked = false;
        }
    }
}

