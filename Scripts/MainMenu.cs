using UnityEngine;
using UnityEngine.Networking;


public class MainMenu : MonoBehaviour
{
    public TMPro.TMP_InputField Ip;
    private void Start()
    {
      //  manager.StartServer();
    }
    public NetworkManager manager;
    public LoadScene SceneLoad;
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void NewServer()
    {
        SceneLoad.DeleteObjectsOfFile(true);
        manager.StartHost();
    }
    public void OldServer()
    {
        SceneLoad.DeleteObjectsOfFile(false);
        manager.StartHost();
    }

    public void JoinLanServer()
    {
        manager.StartClient();
    }
    public void JoinInternetServer()
    {
        manager.networkAddress = Ip.text;
        manager.StartClient();
    }
}
