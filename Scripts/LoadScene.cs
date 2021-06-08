using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    List<string> ObjectsToBeDestroyed = new List<string>();
    FileStream file;
    BinaryFormatter b;

    public void DeleteObjectsOfFile(bool neos)
    {
        if (neos)
        {
            File.Delete("load.txt");
            file = File.Open("load.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            b = new BinaryFormatter();
            ObjectsToBeDestroyed = new List<string>();
            b.Serialize(file, ObjectsToBeDestroyed);
            file.Close();
            File.Delete("Accounts.txt");
            file = File.Open("Accounts.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            b = new BinaryFormatter();
            ObjectsToBeDestroyed = new List<string>();
            b.Serialize(file, ObjectsToBeDestroyed);
            file.Close();
            return;
        }
        try
        {
            file = File.Open("load.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            b = new BinaryFormatter();
            if (true)
            {
                ObjectsToBeDestroyed = b.Deserialize(file) as List<string>;
                foreach (string temp in ObjectsToBeDestroyed)
                {
                    foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                        if (go.name == temp && go.scene.IsValid())
                        {
                            Destroy(go);
                        }
                }
                file.Close();
            }
        }
        catch
        {
            Debug.LogError("COULD NOT INSTATIATE SCENE");
            file.Close();
        }
    }


    public void AddToList(string name)
    {
        file = File.Open("load.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        b = new BinaryFormatter();
        foreach(string temp in ObjectsToBeDestroyed)
        {
            if (temp == name)
            {
                return;
            }
        }
        ObjectsToBeDestroyed.Add(name);
        b.Serialize(file, ObjectsToBeDestroyed);
        file.Close();
    }
}
 