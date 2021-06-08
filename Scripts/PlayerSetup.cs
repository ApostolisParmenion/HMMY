using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using TMPro;
using System;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    public Behaviour[] compomentsToDisable;
    public GameObject[] objectsToControl;
    public TMP_InputField Username;
    public TMP_InputField Password;
    public TMP_InputField RegUsername;
    public TMP_InputField RegPassword;
    public TMP_InputField ConPassword;
    public Text TextUsername; 
    public string USERNAME;
    public bool isserver;
    FileStream file;
    BinaryFormatter b;
    List<PlayerInfo> AllAccounts = new List<PlayerInfo>();
    public GameObject LogInError, RegError, Success, LoginCanvas, sceneCamera;
    private void Start()
    {
        TextUsername.text = "new User";
        if (isServer)
            isserver = true;
        else
            isserver = false;
        if (!isLocalPlayer)
        {
            for (int i = 0; i < compomentsToDisable.Length; i++)
            {
                compomentsToDisable[i].enabled = false;
            }
        }
        else
        {
            GameObject.Find("SceneSun").SetActive(false);
            LoginCanvas.SetActive(true);
            GetComponent<MovementInput>().enabled = false;
            GetComponent<GuardRotation>().enabled = false;
            GetComponent<Attack>().enabled = false;
            GetComponent<Inventory>().enabled = false;
            GetComponentInChildren<HealthControl>().enabled = false;
            GetComponentInChildren<HungerControl>().enabled = false;
            GetComponentInChildren<CameraMovement>().enabled = false;


            Cursor.visible = true;
        }

    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        GameManager.RegisterPlayer(_netID, GetComponent<Player>());
    }
    private void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
        GameManager.UnRegisterPlayer(transform.name);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
    }

    void OnDisconnectedFromServer()
    {
        GameManager.UnRegisterPlayer(transform.name);
    }
    public override void PreStartClient()
    {
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
    }
    public void Register()
    {
        CmdRegister(gameObject, RegUsername.text, RegPassword.text, ConPassword.text);
    }
    [Command]
    void CmdRegister(GameObject obj, string RegUsername, string RegPassword, string ConPassword)
    {
        if(RegPassword != ConPassword)
        {
            RpcFailReg(obj);
            return;
        }
        RegError.SetActive(false);
        PlayerInfo temp = new PlayerInfo();
        temp.Username = RegUsername;
        temp.Password = RegPassword;
        temp.TotalExp = 0;
        temp.items = "";
        try
        {
            file = File.Open("Accounts.txt", FileMode.OpenOrCreate, FileAccess.Read);
            b = new BinaryFormatter();
            try
            {
                AllAccounts = (List<PlayerInfo>)b.Deserialize(file);
                foreach (var temp2 in AllAccounts)
                {
                    if (temp2.Username == temp.Username)
                    {
                        if (file != null)
                            file.Close();
                        RpcFailReg(obj);
                        return;
                    }
                }
            }
            catch 
            {
                print("Failed to desirialize");
            }
            file.Close();
            AllAccounts.Add(temp);
            file = File.Open("Accounts.txt", FileMode.OpenOrCreate, FileAccess.Write);
            b.Serialize(file, AllAccounts);
            if (file != null)
                file.Close();
            RpcSuccessReg(obj);
        }
        catch
        {
            RegError.SetActive(true);
            if (file != null)
                file.Close();
        }

    }
    [ClientRpc]
    void RpcSuccessReg(GameObject obj)
    {
        if (obj.name == gameObject.name)
        {
            print("success");
            Success.SetActive(true);
        }
    }
    void RpcFailReg(GameObject obj)
    {
        if (obj.name == gameObject.name)
        {
            print("regError");
            RegError.SetActive(true);
        }
    }
    public void Login()
    {
        CmdLogin(gameObject, Username.text, Password.text);
    }

    [Command]
    public void CmdLogin(GameObject obj, string Username, string Password)
    {
        try
        {
            if (AllAccounts.Count == 0)
            {
                file = File.Open("Accounts.txt", FileMode.OpenOrCreate, FileAccess.Read);
                b = new BinaryFormatter();
                AllAccounts = b.Deserialize(file) as List<PlayerInfo>;
                file.Close();
            }

            foreach (var temp2 in AllAccounts)
            {
                if (temp2.Username == Username && temp2.Password == Password)
                {
                    obj.GetComponent<PlayerSetup>().USERNAME = Username;
                    RpcSuccessLogin(obj, temp2);
                    return;
                }
            }
        }
        catch
        {
            file.Close();
        }
        this.Username.text = "";
        RpcFailLogin(obj);
        return;

    }
    [ClientRpc]
    void RpcSuccessLogin(GameObject obj, PlayerInfo accountInfo)
    {
        obj.GetComponent<PlayerSetup>().USERNAME = accountInfo.Username;
        TextUsername.text = USERNAME;
        if (obj.name == gameObject.name && isLocalPlayer)
        {
            print("success Login");
            GetComponent<MovementInput>().enabled = true;
            GetComponent<GuardRotation>().enabled = true;
            GetComponent<Attack>().enabled = true;
            GetComponent<Inventory>().enabled = true;
            GetComponentInChildren<HealthControl>().enabled = true;
            GetComponentInChildren<HungerControl>().enabled = true;

            LoginCanvas.SetActive(false);
            Cursor.visible = false;
            GetComponentInChildren<ExpControl>().expGained = accountInfo.TotalExp;
            GetComponentInChildren<ExpControl>().gotExp = true;
            sceneCamera = GameObject.Find("SceneCamera");
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
            GetComponentInChildren<Camera>().enabled = true;
            GetComponentInChildren<AudioListener>().enabled = true;
            GetComponentInChildren<CameraMovement>().enabled = true;

            foreach (var itemsToLoad in accountInfo.items.Split(','))
            {
                foreach (var temp in GetComponent<PickUp>().items)
                {
                    if (temp.name == itemsToLoad)
                    {
                        gameObject.GetComponent<Player>().addToInventory(temp);

                        break;
                    }
                }
            }
        }
    }
    [ClientRpc]
    void RpcFailLogin(GameObject obj)
    {
        if (obj.name == gameObject.name)
        {
            print("Loginfail");
            LogInError.SetActive(true);
        }
    }

}
