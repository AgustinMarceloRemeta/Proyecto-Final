using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CourseController : MonoBehaviour
{
    public List<StudentData> students;
    public string nameCourse;
    public TextMeshProUGUI nameText;

    public void OpenCourse()
    {
        StudentsListManager.instance.SetList(students);
        DataManager.instance.courses.SetActive(false);
        DataManager.instance.studentsPanel.SetActive(true);
    }
}
