using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelCourseController : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI textName;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string text) => textName.text = text;

    public void OpenStudents()
    {
        DataManager.instance.SetCourses();
        StudentsListManager.instance.SetList(CoursesManager.instance.actualCourse.students);
        PanelController.instance.ShowPanel(CoursesManager.instance.studentsPanel);
    }

}
