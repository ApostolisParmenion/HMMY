using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public List<GameObject> weapons = new List<GameObject>();
    public Inventory inventory;
    [SyncVar]
    string EquipSword;
    [SyncVar]
    public bool deadSoReturn = false;

    public void addToInventory(Item item)
    {
        inventory.Add(item);

    }
    public void UpdateDamage(float dammage, string Sword)// updates the damage value of the player
    {
        EquipSword = Sword;
    }
    public void TransferDamage(GameObject obj, int damage) //transfers damage to animal
    {
        if (obj.name == gameObject.name)
        {
            //RpcTransferDamageToPlayer(obj, damage);
            obj.GetComponentInChildren<HealthControl>().damageFromAttack = damage;
            obj.GetComponentInChildren<HealthControl>().attFlag = true;

        }

    }

    [ClientRpc]
    public void RpcTransferDamageToPlayer(GameObject obj, int damage)
    {
        obj.GetComponentInChildren<HealthControl>().damageFromAttack = damage;
        obj.GetComponentInChildren<HealthControl>().attFlag = true;
    }
    [ClientRpc]
    public void RpcEquipWeaponOnAllClients(string itemName)
    {
        gameObject.GetComponentInChildren<HealthControl>().Damage.text = "0";
        foreach (var temp in weapons)
        {
            if (temp.name == itemName)
            {
                inventory.EquipedSword = itemName;
                temp.SetActive(true);
                gameObject.GetComponentInChildren<HealthControl>().Damage.text = temp.GetComponent<WeaponDamage>().damage.ToString();
            }
            else
            {
                temp.SetActive(false);
            }
        }
    }

    [ClientRpc]
    public void RpcUnEquipWeaponOnAllClients(string itemName)
    {
        gameObject.GetComponentInChildren<HealthControl>().Damage.text = "0";
        foreach (var temp in weapons)
        {
            temp.SetActive(false);
        }
    }


}
