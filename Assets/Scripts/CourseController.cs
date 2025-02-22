using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CourseController : MonoBehaviour
{
    public List<StudentData> students;
    public DataCourse dataCourse;
    public TextMeshProUGUI nameText;
    public float noteMax;
    public string initialDate;
    private void Start()
    {
        foreach (StudentData item in students) DataManager.instance.SetAverages(item,noteMax);
    }
    public void OpenCourse()
    {
        CoursesManager.instance.actualCourse = this;
        StudentsListManager.instance.SetList(students);
        PanelController.instance.ShowPanel(CoursesManager.instance.courseActualPanel);
        CoursesManager.instance.panelCourseController.SetText(nameText.text);
    }

    public void ResetCourse()
    {
        foreach (var item in students)
        {
            item.attendancePercentage = 0;
            item.absence= 0;
            item.attendances = 0;
            item.notes.Clear();
            item.noteAverage= 0;
            item.notePercentage = 0;
            item.historialDiasList.Clear();
        }
    }
}
