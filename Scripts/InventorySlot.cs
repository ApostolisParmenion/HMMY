
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Item item;
    public Button removeButton;
    public HealthControl healthScript;
    public HungerControl HungerScript;
    public Inventory inventory;
    public Player PlayerScript;
    public void AddItem(Item NewItem)
    {
        item = NewItem;
        item.name = item.name.Replace("(Clone)", "");
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        inventory.Remove(item);
    }
    public void UseItem()
    {
        if (item != null)
        {
            if (item.Category == "Sword")
            {
                inventory.CmdEquipWeapon(item.name);
            }
            else if (item.Category == "Food")
            {
                GameObject[] ArrayOfObjects = inventory.ItemsToPickUp.ToArray();
                for (int i = 0; i < ArrayOfObjects.Length; i++)
                {
                    if (ArrayOfObjects[i].name == item.name)
                    {
                        healthScript.eaten = true;
                        HungerScript.eaten = true;
                        print("Eating " + item.name);
                        inventory.Remove(item, false);
                        break;

                    }
                }
            }
        }
    }
}
