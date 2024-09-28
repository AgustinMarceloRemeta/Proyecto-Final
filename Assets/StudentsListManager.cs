using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StudentsListManager : MonoBehaviour
{
    public static StudentsListManager instance;
    string inicialText;
    [SerializeField] TextMeshProUGUI lastNameText, nameText, attendancesText, noteText;

    private void Awake()
    {
        instance = this;
        inicialText= lastNameText.text;
    }
    void Start()
    {
        
    }

    public void SetList(List<StudentData> students)
    {
        lastNameText.text = SetLastNames(students);
        nameText.text = SetOtherText(students, "name");
        attendancesText.text = SetOtherText(students, "attendancePercentage");
        noteText.text = SetOtherText(students, "noteAverage");
    }

    public string SetLastNames(List<StudentData> students)
    {
        string newText = inicialText + "\n";
        foreach (StudentData student in students)
        {
            newText += "  " + student.lastName + "\n \n \n";
        }
        return newText;
    }

    public string SetOtherText(List<StudentData> students, string nameProperty)
    {
        string newText = string.Empty;
        foreach (StudentData student in students)
        {
            var studentType = student.GetType();
            var property = studentType.GetField(nameProperty);
            if (property != null)
            {
                var value = property.GetValue(student);
                int wordCount = value.ToString().Split(' ').Length;
                if (nameProperty == "attendancePercentage") newText += value + "% \n \n \n";
                else
                {
                    if (wordCount > 1)
                    {
                        newText += value + "\n \n ";
                    }
                    else
                    {
                        newText += value + "\n \n \n";
                    }
                }
            }
            else return string.Empty;
        }
        return newText;
    }
}
