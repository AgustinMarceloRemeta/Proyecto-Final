using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
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

    float SetNoteAverage(List <Note> note, bool percentage, float noteMax)
    {
        float numberOfNote = 0;
        foreach (Note item in note)
        {
            numberOfNote += item.value;
        }
        if(percentage) return note.Count > 0 ? (numberOfNote*100) / (note.Count*noteMax)  : 0 ;
        else return note.Count > 0 ? numberOfNote / note.Count : 0; ;
    }

    public void SetAverages(StudentData studentData,float noteMax)
    {
        studentData.attendancePercentage = SetAttendanceAverage(studentData.attendanceBools, "percentage");
        studentData.attendances = SetAttendanceAverage(studentData.attendanceBools, "attendances");
        studentData.absence = SetAttendanceAverage(studentData.attendanceBools, "absence");
        studentData.totalDays = SetAttendanceAverage(studentData.attendanceBools, "totalDays");
        studentData.noteAverage = SetNoteAverage(studentData.notes, false, noteMax);
        studentData.notePercentage = SetNoteAverage(studentData.notes, true, noteMax);
    }

    public void OrderStudents(List<StudentData> studentData)
    {
        studentData.Sort((student1, student2) => student1.lastName.CompareTo(student2.lastName));
    }

    public void SetCourses()
    {
        foreach (StudentData item in LoadData.instance.data.students)
        {
            //esto es temporal hasta que haga un json de cursos o algo por el estilo;
            if (!CoursesManager.instance.coursesNames.Contains(item.course)) CoursesManager.instance.NewCourse(item.course, 10);
            CoursesManager.instance.AddNewStudent(item);
        }
    }

}
