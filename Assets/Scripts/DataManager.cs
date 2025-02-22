using System.Collections.Generic;
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
        foreach (StudentData item in LoadData.instance.data.students)
        {
            if (!CoursesManager.instance.coursesNames.Contains(item.course.courseName)) CoursesManager.instance.NewCourse(item.course, 10,"2025-03-01");
            AddStudentController.instance.AddNewStudent(item);
        }
    }

}
