using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

public class CoursesManager : MonoBehaviour
{
    public static CoursesManager instance;
    [SerializeField] GameObject prefabCourse;
    Stack<GameObject> coursesList = new Stack<GameObject>();
    public List<string> coursesNames = new List<string>();
    [SerializeField] Vector3 originalPosition;
    [SerializeField] float distanceObjects;
    [SerializeField] GameObject parent;
    [SerializeField] GameObject panel;
    public GameObject studentsPanel;
    public GameObject courses;
    [SerializeField] TMP_InputField nameText;
    [SerializeField] TMP_InputField noteText;
    [SerializeField] TextMeshProUGUI alertText;
    [SerializeField] GameObject settingsPanel;
    private void Awake()
    {
        instance = this;
    }

    public void SetNewCourse()
    {
        float newNoteMax = 0;
        print("hola" == noteText.text.Normalize());
        try
        {
            newNoteMax = int.Parse(noteText.text.Trim(), CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
            print("Ingrese un numero");
            return;
        }
                    NewCourse(nameText.text, newNoteMax);
            courses.SetActive(true);
            settingsPanel.SetActive(false);

    }

    public void NewCourse(string course, float noteMax)
    {
        GameObject newCourse = GameObject.Instantiate(prefabCourse);
        newCourse.transform.SetParent(parent.transform);
        if (coursesList.Count > 0) newCourse.transform.localPosition = coursesList.Peek().transform.localPosition - new Vector3(0, distanceObjects, 0);
        else newCourse.transform.localPosition = originalPosition;
        coursesList.Push(newCourse);
        coursesNames.Add(course);
        CourseController courseController = newCourse.GetComponent<CourseController>();
        courseController.nameCourse = course;
        courseController.nameText.text = course;
        courseController.noteMax = noteMax;
    }

    public void AddNewStudent(StudentData student)
    {
        foreach (var item in coursesList)
        {
            CourseController courseController = item.GetComponent<CourseController>();
            if (courseController.nameCourse == student.course)
            {
                courseController.students.Add(student);
                break;
            }
        }
    }
}