using UnityEngine;
using UnityEngine.Networking;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventorY;
    public Transform itemsParent;
    public InventorySlot[] slots;

    public void Start()
    {
        UpdateUI();
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }
    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventorY.items.Count)
            {
                slots[i].AddItem(inventorY.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
