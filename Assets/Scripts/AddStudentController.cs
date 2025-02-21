using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Globalization;

public class AddStudentController : MonoBehaviour
{
    public List<Note> notes = new List<Note>();
    public static AddStudentController instance;
    [SerializeField] TextMeshProUGUI nameText, lastNameText;

    private void Awake()
    {
        instance = this;
    }
    public void CreateNewStudent()
    {
        StudentData studentData = new StudentData();
        studentData.name = nameText.text;
        studentData.lastName = lastNameText.text;
        studentData.course = CoursesManager.instance.actualCourse.nameCourse;
        studentData.notes = notes;
        AddNewStudent(studentData);
        LoadData.instance.data.students.Add(studentData);
        ResetScript();
    }

    public void AddNewStudent(StudentData student)
    {
        foreach (var item in CoursesManager.instance.coursesList)
        {
            CourseController courseController = item.GetComponent<CourseController>();
            if (courseController.nameCourse == student.course)
            {
                courseController.students.Add(student);
                break;
            }
        }
    }

    public void ResetScript()
    {
         notes.Clear();
    }
}
