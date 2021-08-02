using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Patterns.Observer;

public class GameData : MonoBehaviour
{
    public static GameData self;
    public Dictionary<string, SaveData> saveData = new Dictionary<string, SaveData>();

    private void Start()
    {
        if (!self)
        {
            self = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private string GetFileName(int saveid)
    {
        return Application.persistentDataPath + "/save" + saveid.ToString() + ".sav";
    }

    public void Save(int saveid)
    {
        BinaryFormatter bf = new BinaryFormatter();
        Debug.Log("saving to path " + GetFileName(saveid));
        FileStream file = File.Create(GetFileName(saveid));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load(int saveid)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(GetFileName(saveid), FileMode.Open);
        saveData = (Dictionary<string, SaveData>)bf.Deserialize(file);
        foreach (var data in saveData)
        {
            this.Notify(Message.System_LoadData, data.Key, data.Value);
        }
        file.Close();
    }
}

[Serializable]
public class SaveData
{
    public string guid = Guid.NewGuid().ToString();
}
