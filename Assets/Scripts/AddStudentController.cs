using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddStudentController : MonoBehaviour
{
    public List<Note> notes = new List<Note>();
    public static AddStudentController instance;
    [SerializeField] TextMeshProUGUI nameText, lastNameText, referenceText, noteValueText;

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

    public void AddNote(string reference, float noteValue)
    {
        Note note = new Note();
        note.referencia = referenceText.text;
        //TODO: CONVERTIR DE STRING A FLOAT;
        note.value = noteValueText.text;
        notes.Add(note);
    }

    public void ResetScript()
    {
         notes.Clear();
    }
}
