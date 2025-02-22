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
    public StudentData CreateNewStudent()
    {
        StudentData studentData = new StudentData();
        studentData.name = nameText.text;
        studentData.lastName = lastNameText.text;
        studentData.course = CoursesManager.instance.actualCourse.dataCourse;
        studentData.notes = notes;
        AddNewStudent(studentData);
        LoadData.instance.data.students.Add(studentData);
        ResetScript();
        return studentData;
    }

    public void AddNewStudent(StudentData student)
    {
        foreach (var item in CoursesManager.instance.coursesList)
        {
            CourseController courseController = item.GetComponent<CourseController>();
            if (courseController.dataCourse == student.course)
            {
                courseController.students.Add(student);
                break;
            }
        }
    }

    public void CreateStudentButton()
    {
        CreateNewStudent();
    }
    public void AddNotes()
    {
        NoteManager.instance.selectedStudent = CreateNewStudent();
        NoteManager.instance.UpdateText();
        PanelController.instance.ShowPanelWithoutSaving(NoteManager.instance.notePanel);
    }
    public void ResetScript()
    {
         notes.Clear();
    }
}
