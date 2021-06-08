using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Inventory : NetworkBehaviour
{
    public List<Item> items = new List<Item>();
    public List<GameObject> ItemsToPickUp = new List<GameObject>();
    public GameObject player;
    [SyncVar]
    public string EquipedSword;
    public GameObject invetoryUI;
    public InventoryUI inventoryUI;
    public Player _Player;
    public List<GameObject> weapons = new List<GameObject>();
    public NetworkManager manager;
    public GameObject OptionPanel;
    public bool canAttack;

    private void Start()
    {
        EquipedSword = null;
        invetoryUI.SetActive(false);
        canAttack = true;
    }
    #region Singleton

    #endregion
    private void Update()
    {
        if (Input.GetKeyDown("i") || (Input.GetButtonDown("Cancel") && invetoryUI.activeInHierarchy))
        {
            invetoryUI.SetActive(!invetoryUI.activeInHierarchy);
            if (invetoryUI.activeInHierarchy)
            {
                canAttack = false;
                Cursor.visible = true;
            }
            else
            {
                canAttack = true;
                Cursor.visible = false;
            }
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            OptionPanel.active = !OptionPanel.active;
            if (OptionPanel.active)
            {
                canAttack = false;
                Cursor.visible = true;

            }
            else
            {
                canAttack = true;
                Cursor.visible = false;
            }
        }
    }
    [Client]
    public void Add(Item item)
    {
        items.Add(item);
        inventoryUI.UpdateUI();
    }
    [Client]
    public void Remove(Item item, bool notDrop = true)
    {
        if (notDrop)
        {
            CmdSpawnObject(item.name);
            foreach (var temp in weapons)
            {
                if (temp.name == item.name)
                {
                    if (EquipedSword == item.name)
                    {
                        EquipedSword = null;
                        temp.SetActive(false);
                        CmdUnEquipWeapon(item.name);
                    }
                    break;
                }
            }
        }
        items.Remove(item);
        inventoryUI.UpdateUI();
        CmdRemoveFromDatabase(item.name);
    }

    [Command]
    public void CmdEquipWeapon(string itemName)
    {
        gameObject.GetComponentInChildren<HealthControl>().Damage.text = "0";
        foreach (var temp in weapons)
        {
            if (temp.name == itemName)
            {
                EquipedSword = itemName;
                temp.SetActive(true);
                _Player.RpcEquipWeaponOnAllClients(EquipedSword);
                gameObject.GetComponentInChildren<HealthControl>().Damage.text = temp.GetComponent<WeaponDamage>().damage.ToString();
            }
            else
            {
                temp.SetActive(false);
            }
        }
    }
    [Command]
    public void CmdUnEquipWeapon(string itemName)
    {
        gameObject.GetComponentInChildren<HealthControl>().Damage.text = "0";
        foreach (var temp in weapons)
        {
            if (temp.name == itemName)
            {
                temp.SetActive(false);
                _Player.RpcUnEquipWeaponOnAllClients(itemName);
                break;
            }

        }
    }

    [Command]
    public void CmdSpawnObject(string name)
    {
        GameObject obj = (GameObject)Instantiate(Resources.Load(name, typeof(GameObject)), player.transform.position + 4 * Vector3.up + 3 * Vector3.forward, player.transform.rotation);
        obj.name = name;
        NetworkServer.Spawn(obj);
    }
    [Command]
    public void CmdRemoveFromDatabase(string itemName)
    {
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
                    string[] items = AllAccounts[i].items.Split(',');
                    for (int j = 0; j < items.Length; j++)
                    {
                        if (items[j] == itemName)
                        {
                            items[j] = "";
                            break;
                        }
                    }
                    AllAccounts[i].items = "";
                    for (int j = 0; j < items.Length; j++)
                    {
                        if (items[j] != "")
                        {
                            AllAccounts[i].items += items[j] + ',';
                        }
                    }
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


    public void ReloadScene()
    {
        Cursor.visible = true;
        manager = GameObject.Find("_NetworkManager").GetComponent<NetworkManager>();
        if (isServer)
            manager.StopHost();
        else
        {
            manager.StopClient();
        }
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Continue()
    {
        OptionPanel.active = false;
        canAttack = true;
        Cursor.visible = false;
    }
}
