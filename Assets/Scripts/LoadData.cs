using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class LoadData : MonoBehaviour
{
    public JsonData data;
    public static LoadData instance;
    [SerializeField] TextMeshProUGUI lastNameText, nameText, attendancesText, noteText;
    private void Awake()
    {
        instance = this;
        data = ReturnData();
    }

    private void Start()
    {
        foreach (StudentData item in data.students) DataManager.instance.SetAverages(item);       
        DataManager.instance.OrderStudents(data.students);
        lastNameText.text = DataManager.instance.SetLastNames(data.students, lastNameText.text);
        nameText.text = DataManager.instance.SetOtherText(data.students, "name");
        attendancesText.text = DataManager.instance.SetOtherText(data.students, "attendancePercentage");
        noteText.text = DataManager.instance.SetOtherText(data.students, "noteAverage");
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
