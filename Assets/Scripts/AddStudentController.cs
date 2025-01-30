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
    [SerializeField] TextMeshProUGUI nameText, lastNameText, referenceText, noteValueText, notesText, alertText;

    private void Awake()
    {
        instance = this;
    }
    public void CreateNewStudent()
    {
        StudentData studentData = new StudentData();
        studentData.name = nameText.text;
        studentData.lastName = lastNameText.text;
        studentData.course = CoursesManager.instance.actualCourse;
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

    public void AddNote()
    {
        Note note = new Note();
        if (string.IsNullOrWhiteSpace(referenceText.text)) //verifica si la referencia es valida
        {
            Alert.instance.StartAlert(AlertTexts.textInvalid, alertText);
            return;
        }
        string cleanValueText = noteValueText.text.Replace("\u200B", "");
        try //verifica si la nota es valida
        {
            note.value = int.Parse(cleanValueText.Trim(), CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
            Alert.instance.StartAlert(AlertTexts.valueInvalid, alertText);
            return;
        }
        note.referencia = referenceText.text;

        notes.Add(note);
        UpdateText();
    }

    public void UpdateText()
    {
        notesText.text = "Notas: ";
        foreach (var item in notes)
        {
            notesText.text += item.referencia + " " + item.value + " / ";
        }
    }

    public void ResetScript()
    {
         notes.Clear();
    }
}
