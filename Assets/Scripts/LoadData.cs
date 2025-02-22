using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class LoadData : MonoBehaviour
{
    public JsonData data;
    public static LoadData instance;

    private void Awake()
    {
        instance = this;
        data = ReturnData();

    }

    private void Start()
    {
        if(data.students.Count>0)DataManager.instance.OrderStudents(data.students);
        DataManager.instance.SetCourses();
        print(Application.persistentDataPath + "/StudentData.json");
    }

    public JsonData ReturnData ()
    {
        JsonData data = new JsonData();
        data.students = new List<StudentData>();
        string path = Application.persistentDataPath + "/StudentData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            JsonData studentData = JsonUtility.FromJson<JsonData>(json);
            return studentData;
        }
        else return data;
    }

    private void OnDisable()
    {
        SaveData.instance.SaveStudentData(data);
    }
}
