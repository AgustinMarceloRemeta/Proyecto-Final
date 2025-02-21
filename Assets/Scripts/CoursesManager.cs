using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using System.Linq;

public class CoursesManager : MonoBehaviour
{
    public static CoursesManager instance;
    [SerializeField] GameObject prefabCourse;
    public Stack<GameObject> coursesList = new Stack<GameObject>();
    public List<string> coursesNames = new List<string>();
    List<CourseController> courseControllers = new List<CourseController>();
    [SerializeField] Vector3 originalPosition;
    [SerializeField] float distanceObjects;
    [SerializeField] GameObject parent;
    [SerializeField] GameObject panel;
    public GameObject studentsPanel;
    public GameObject coursesPanel;
    public GameObject courseActualPanel;
    [SerializeField] TMP_InputField nameText;
    [SerializeField] TMP_InputField noteText;
    [SerializeField] TextMeshProUGUI alertText;
    [SerializeField] GameObject settingsPanel;
    public CourseController actualCourse;
    public PanelCourseController panelCourseController;
    public DateDropdownController dateDropdownController;
    private void Awake()
    {
        instance = this;
    }

    public void SetActualCouse(CourseController course)=> actualCourse = course;

    public void SetNewCourse()
    {
        float newNoteMax = 0;
        bool courseNameCorrect = !coursesNames.Contains(nameText.text) && nameText.text != string.Empty;
       if(courseNameCorrect) 
        try
        {
            newNoteMax = int.Parse(noteText.text.Trim(), CultureInfo.InvariantCulture);
                if(newNoteMax <= 0)
                {
                    print("numero invalido");
                    return;
                }
        }
        catch (FormatException)
        {
            print("Ingrese un numero");
            return;
        }
        else
        {
            print("Ingrese un nombre correcto");
            return;
        }
        NewCourse(nameText.text, newNoteMax,dateDropdownController.GetSelectedDate());
        coursesPanel.SetActive(true);
        settingsPanel.SetActive(false);

    }

    public void NewCourse(string course, float noteMax, string newDate)
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
        courseController.initialDate = newDate;
        courseControllers.Add(courseController);
    }

    public CourseController GetCourse(string course)
    {
        foreach (CourseController item in courseControllers)
        {
            if(item.nameCourse == course) return item;
        }
        return null;
    }
}