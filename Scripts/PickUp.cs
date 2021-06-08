using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#pragma warning disable CS0618 // Type or member is obsolete
public class PickUp : NetworkBehaviour
#pragma warning restore CS0618 // Type or member is obsolete
{
    public RaycastHit hit;
    public LayerMask InteractLayer;
    public float distance = 10f;
    public string ItemsName;
    GameObject objectToBeDestroied;
    public GameObject cam;
    public List<Item> items = new List<Item>();
    public Inventory inventory;
    public InventoryUI InventoryUpdate;
    public LoadScene LoadScene;
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            PickUpObject();
        }
    }

    [Client]
    void PickUpObject()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance, InteractLayer))
        {
            ItemsName = hit.collider.gameObject.name;
            objectToBeDestroied = GameObject.Find(ItemsName);
            string _netID = GetComponent<NetworkIdentity>().netId.ToString();
            Destroy(objectToBeDestroied);
            Player _player = GameManager.GetPlayer("Player " + _netID);

            foreach (var temp in items)
            {
                if (temp.name == objectToBeDestroied.name.Replace("(Clone)", ""))
                {
                    if (temp.Category != "Food")
                        CmdPickUp(_player.gameObject, objectToBeDestroied, true);
                    else
                        CmdPickUp(_player.gameObject, objectToBeDestroied, false);
                    _player.addToInventory(temp);
                    break;
                }
            }
        }
    }
    [Command]
    public void CmdPickUp(GameObject player, GameObject objectToBeDestroied, bool isWeapon)
    {
        LoadScene = GameObject.Find("_GameManager").GetComponent<LoadScene>();
        Destroy(objectToBeDestroied);

        if (isWeapon)
        {
            LoadScene.AddToList(objectToBeDestroied.name.Replace("(Clone)", ""));
        }
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
                if (AllAccounts[i].Username == player.GetComponent<PlayerSetup>().USERNAME)
                {
                    AllAccounts[i].items += objectToBeDestroied.name.Replace("(Clone)", "") + ',';
                }
            }
            file = File.Open("Accounts.txt", FileMode.OpenOrCreate, FileAccess.Write);
            b.Serialize(file, AllAccounts);
            file.Close();
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
            file.Close();
        }
    }

}
