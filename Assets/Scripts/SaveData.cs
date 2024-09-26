using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveData : MonoBehaviour
{
    public static SaveData instance;

    private void Awake()
    {
        instance = this;
    }
    public void SaveStudentData(JsonData data)
    {
        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + "/StudentData.json";
        File.WriteAllText(path, json);
    }
}

