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
    [SerializeField] GameObject curso;

    private void Awake()
    {
        instance = this;
    }
    public StudentData CreateNewStudent()
    {
        StudentData studentData = new StudentData();
        if(nameText.text == string.Empty || lastNameText.text == string.Empty)
        {
            Alert.instance.StartAlert(AlertTexts.emptyText);
            return null;
        }    
        studentData.name = nameText.text;
        studentData.lastName = lastNameText.text;
        studentData.course = CoursesManager.instance.actualCourse.dataCourse;
        studentData.notes = notes;
        AddNewStudent(studentData);
        LoadData.instance.data.students.Add(studentData);
        ResetScript();
        RewardController.instance.AddReward(Reward.firtsStudent);
        return studentData;
    }

    public void AddNewStudent(StudentData student)
    {
        foreach (var item in CoursesManager.instance.coursesList)
        {
            CourseController courseController = item.GetComponent<CourseController>();
            if (courseController.dataCourse.courseName == student.course.courseName)
            {
                courseController.students.Add(student);
                break;
            }
        }
    }

    public void CreateStudentButton()
    {
        StudentData studentData =  CreateNewStudent();
        if(studentData!= null)
        PanelController.instance.ShowPanelWithoutSaving(curso);
    }
    public void AddNotes()
    {
        StudentData studentData = CreateNewStudent();
        if (studentData == null) return;
        NoteManager.instance.selectedStudent = studentData;
        NoteManager.instance.UpdateText();
        PanelController.instance.ShowPanelWithoutSaving(NoteManager.instance.notePanel);
    }
    public void ResetScript()
    {
         notes.Clear();
    }

    public void AddAttendances()
    {
        StudentData studentData = CreateNewStudent();
        if (studentData == null) return;
        PanelController.instance.ShowPanel(CalendarioController.Instance.panel);
        CalendarioController.Instance.SetInitialDate(CoursesManager.instance.GetCourse(studentData.course.courseName).initialDate);
        CalendarioController.Instance.student = studentData;
        CalendarioController.Instance.UpdateHistorialDias(DictionaryConverter.ConvertDiaInfoEntriesToDictionary(studentData.historialDiasList));
    }
}
