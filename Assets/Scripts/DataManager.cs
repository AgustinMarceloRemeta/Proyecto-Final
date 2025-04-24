using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private void Awake()
    {
        instance = this;
    }
    float SetAttendanceAverage(StudentData student)
    {
        float numbersOfAttendance = student.absence + student.attendances;

        return student.attendances > 0 ? (student.attendances  * 100) / numbersOfAttendance : 0;
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
        studentData.attendancePercentage = SetAttendanceAverage(studentData);
        studentData.noteAverage = SetNoteAverage(studentData.notes, false, noteMax);
        studentData.notePercentage = SetNoteAverage(studentData.notes, true, noteMax);
    }

    public void OrderStudents(List<StudentData> studentData)
    {
        studentData.Sort((student1, student2) => student1.lastName.CompareTo(student2.lastName));
    }

    public void SetCourses()
    {
        foreach (var course in CoursesManager.instance.coursesList) course.GetComponent<CourseController>().students.Clear();
       if(LoadData.instance.courses == null) LoadData.instance.courses = new List<DataCourse> ();
        
        foreach (DataCourse item in LoadData.instance.courses)
        {
            if (!CoursesManager.instance.coursesNames.Contains(item.courseName)) CoursesManager.instance.NewCourse(item,item.initialCourse);
        }
        foreach (StudentData item in LoadData.instance.data.students)
        {
            AddStudentController.instance.AddNewStudent(item);
        }
    }

    public void CheckAttendances(List<StudentData> studentData)
    {
        foreach (var item in studentData)
        {
            if (item.course.initialCourse == string.Empty) continue;
            DateTime initial = DateTime.ParseExact(item.course.initialCourse, "yyyy-MM-dd", null);
            List<DiaInfoEntry> newListDay = new List<DiaInfoEntry>(item.historialDiasList);
            foreach (var day in newListDay)
            {
                DateTime actual = new DateTime(day.value.Año, day.value.Mes, day.value.Dia);
                if (actual < initial) item.historialDiasList.Remove(day);
            }
        }
    }
    public void ChekAttendancesButton() => CheckAttendances(LoadData.instance.data.students);
}
