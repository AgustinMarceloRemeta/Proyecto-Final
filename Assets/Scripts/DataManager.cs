using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public float noteMax;
    public static DataManager instance;

    private void Awake()
    {
        instance = this;
    }
    float SetAttendanceAverage(List <bool> attendance, string type)
    {
        int numbersOfAttendance = 0;
        foreach (bool attendanceItem in attendance)
        {
            if(attendanceItem)
            {
                numbersOfAttendance++;
            }
        }

        if (type == "percentage") return attendance.Count > 0 ? (numbersOfAttendance * 100) / attendance.Count : 0;
        else if (type == "attendances") return numbersOfAttendance;
        else if (type == "absence") return attendance.Count - numbersOfAttendance;
        else if (type == "totalDays") return attendance.Count;
        else return 0;
    }

    float SetNoteAverage(List <float> note, bool percentage)
    {
        float numberOfNote = 0;
        foreach (float item in note)
        {
            numberOfNote += item;
        }
        if(percentage) return note.Count > 0 ? (numberOfNote*100) / (note.Count*noteMax)  : 0 ;
        else return note.Count > 0 ? numberOfNote / note.Count : 0; ;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetAverages(LoadData.instance.data.students[0]);
        }
    }

    public void SetAverages(StudentData studentData)
    {
        studentData.attendancePercentage = SetAttendanceAverage(studentData.attendanceBools, "percentage");
        studentData.attendances = SetAttendanceAverage(studentData.attendanceBools, "attendances");
        studentData.absence = SetAttendanceAverage(studentData.attendanceBools, "absence");
        studentData.totalDays = SetAttendanceAverage(studentData.attendanceBools, "totalDays");
        studentData.noteAverage = SetNoteAverage(studentData.notes, false);
        studentData.notePercentage = SetNoteAverage(studentData.notes, true);
    }

    public void OrderStudents(List<StudentData> studentData)
    {
        studentData.Sort((student1, student2) => student1.lastName.CompareTo(student2.lastName));
    }

    public string SetLastNames(List<StudentData> students, string originalText)
    {
        originalText += "\n";
        foreach (StudentData student in students)
        {
            originalText += "  " + student.lastName + "\n \n \n";
        }
        return originalText;
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
                var value= property.GetValue(student);
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
