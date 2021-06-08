
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/item")]
public class Item : ScriptableObject
{
    public string itemName = "New item";
    public string Category = "New Category";
    public Sprite icon = null;



    public virtual void EquipSword()
    {

        Debug.Log("Equiping " + itemName);
        
    }
}
