using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CourseController : MonoBehaviour
{
    public List<StudentData> students;
    public string nameCourse;
    public TextMeshProUGUI nameText;
    public float noteMax;
    public string initialDate;
    private void Start()
    {
        foreach (StudentData item in students) DataManager.instance.SetAverages(item,noteMax);
    }
    public void OpenCourse()
    {
        StudentsListManager.instance.SetList(students);
        PanelController.instance.ShowPanel(CoursesManager.instance.courseActualPanel);
        CoursesManager.instance.panelCourseController.SetText(nameText.text);
        CoursesManager.instance.actualCourse = this;
    }
   
}
