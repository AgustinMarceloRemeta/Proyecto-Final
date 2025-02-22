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
    public DateDropdownController modifyDateDropdownController;
    public TMP_InputField modifyNameText, modifyNoteText;
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
                    Alert.instance.StartAlert(AlertTexts.valueInvalid);

                    return;
                }
        }
        catch (FormatException)
        {
                Alert.instance.StartAlert(AlertTexts.addNumber);
            return;
        }
        else
        {
            Alert.instance.StartAlert(AlertTexts.textInvalid);

            return;
        }
       DataCourse dataCourse = new DataCourse();
        dataCourse.courseName = nameText.text;
        NewCourse(dataCourse, newNoteMax,dateDropdownController.GetSelectedDate());
        coursesPanel.SetActive(true);
        settingsPanel.SetActive(false);

    }

    public void NewCourse(DataCourse course, float noteMax, string newDate)
    {
        GameObject newCourse = GameObject.Instantiate(prefabCourse);
        newCourse.transform.SetParent(parent.transform);
        if (coursesList.Count > 0) newCourse.transform.localPosition = coursesList.Peek().transform.localPosition - new Vector3(0, distanceObjects, 0);
        else newCourse.transform.localPosition = originalPosition;
        coursesList.Push(newCourse);
        coursesNames.Add(course.courseName);
        CourseController courseController = newCourse.GetComponent<CourseController>();
        courseController.dataCourse = course;
        courseController.nameText.text = course.courseName;
        courseController.noteMax = noteMax;
        courseController.initialDate = newDate;
        courseControllers.Add(courseController);
    }

    public CourseController GetCourse(string course)
    {
        foreach (CourseController item in courseControllers)
        {
            if(item.dataCourse.courseName == course) return item;
        }
        return null;
    }

    public void ResetActualCourse()
    {
        actualCourse.ResetCourse();
    }

    public void SetModifiers()
    {
        modifyNameText.text= actualCourse.dataCourse.courseName; ;
        modifyNoteText.text = actualCourse.noteMax.ToString();
        modifyDateDropdownController.SetDateFromString(actualCourse.initialDate);
    }

    public void ModifyActualCourse()
    {
        float newNoteMax = 0;
        bool courseNameCorrect = modifyNameText.text != string.Empty;
        if (courseNameCorrect)
            try
            {
                newNoteMax = int.Parse(modifyNoteText.text.Trim(), CultureInfo.InvariantCulture);
                if (newNoteMax <= 0)
                {
                    Alert.instance.StartAlert(AlertTexts.valueInvalid);
                    return;
                }
            }
            catch (FormatException)
            {
                Alert.instance.StartAlert(AlertTexts.addNumber);
                return;
            }
        else
        {
            Alert.instance.StartAlert(AlertTexts.textInvalid);
            return;
        }
        coursesNames.Remove(actualCourse.dataCourse.courseName);
        actualCourse.dataCourse.courseName = modifyNameText.text;
        actualCourse.noteMax = newNoteMax;
        actualCourse.initialDate = modifyDateDropdownController.GetSelectedDate();
        foreach (var item in actualCourse.students)
        {
            item.course.courseName = modifyNameText.text;
        }
        coursesNames.Add(modifyNameText.text);
        actualCourse.GetComponentInChildren<TextMeshProUGUI>().text = modifyNameText.text;
    }
    public void OpenActualCourse()
    {
        actualCourse.OpenCourse();
    }
    public void CloseActualCourse()
    {
        actualCourse.dataCourse.closed = true;
    }
}