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
        foreach (StudentData item in data.students) DataManager.instance.SetAverages(item);       
        DataManager.instance.OrderStudents(data.students);
        DataManager.instance.SetCourses();
    }

    public void SetAllStudents()
    {
        StudentsListManager.instance.SetList(data.students);
    }
    public JsonData ReturnData ()
    {
        string path = Application.persistentDataPath + "/StudentData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            JsonData studentData = JsonUtility.FromJson<JsonData>(json);
            return studentData;
        }
        else return null;
    }

    private void OnDisable()
    {
        SaveData.instance.SaveStudentData(data);
    }
}
