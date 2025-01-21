using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CourseController : MonoBehaviour
{
    public List<StudentData> students;
    public string nameCourse;
    public TextMeshProUGUI nameText;
    public float noteMax;

    private void Start()
    {
        foreach (StudentData item in students) DataManager.instance.SetAverages(item,noteMax);
    }
    public void OpenCourse()
    {
        StudentsListManager.instance.SetList(students);
        CoursesManager.instance.courses.SetActive(false);
        CoursesManager.instance.studentsPanel.SetActive(true);
    }
   
}
