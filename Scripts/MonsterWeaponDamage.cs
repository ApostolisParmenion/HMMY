using UnityEngine;

public class MonsterWeaponDamage : MonoBehaviour
{
    public int damage;
    public Rigidbody rigi;
    public BoxCollider col;
    public bool WorldObject;
    public Animator MonsterAnimator;
    bool checkIfAlreadyAttacked; // false ama den exw varesei
    void Start()
    {
        rigi = this.GetComponent<Rigidbody>();
        col = this.GetComponent<BoxCollider>();
        GiveValue();
        checkIfAlreadyAttacked = false;
    }
    private void GiveValue()
    {
        col.isTrigger = true;
    }

    private void Update()
    {
        if (!MonsterAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack") && checkIfAlreadyAttacked)
        {
            checkIfAlreadyAttacked = false;
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player") && MonsterAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack") && !checkIfAlreadyAttacked)
        {
            checkIfAlreadyAttacked = true;
            col.gameObject.GetComponent<Player>().TransferDamage(col.gameObject, damage);
        }
    }
}

