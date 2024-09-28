using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public float noteMax;
    public static DataManager instance;
    [Header("Courses")]
    [SerializeField] GameObject prefabCourse;
    Stack<GameObject> coursesList = new Stack<GameObject>();
    List<string> coursesNames = new List<string>();
    [SerializeField] Vector3 originalPosition;
    [SerializeField] float distanceObjects;
    [SerializeField] GameObject parent;
    [SerializeField] GameObject panel;
    public GameObject studentsPanel;
    public GameObject courses;

    private void Awake()
    {
        instance = this;
    }
    float SetAttendanceAverage(List <bool> attendance, string type)
    {
        int numbersOfAttendance = 0;
        foreach (bool attendanceItem in attendance)
        {
            if(attendanceItem)
            {
                numbersOfAttendance++;
            }
        }

        if (type == "percentage") return attendance.Count > 0 ? (numbersOfAttendance * 100) / attendance.Count : 0;
        else if (type == "attendances") return numbersOfAttendance;
        else if (type == "absence") return attendance.Count - numbersOfAttendance;
        else if (type == "totalDays") return attendance.Count;
        else return 0;
    }

    float SetNoteAverage(List <float> note, bool percentage)
    {
        float numberOfNote = 0;
        foreach (float item in note)
        {
            numberOfNote += item;
        }
        if(percentage) return note.Count > 0 ? (numberOfNote*100) / (note.Count*noteMax)  : 0 ;
        else return note.Count > 0 ? numberOfNote / note.Count : 0; ;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (var item in coursesList)
            {
                print(item.transform.localPosition);
                print(item.transform.parent.name);
            }
        }
    }

    public void SetAverages(StudentData studentData)
    {
        studentData.attendancePercentage = SetAttendanceAverage(studentData.attendanceBools, "percentage");
        studentData.attendances = SetAttendanceAverage(studentData.attendanceBools, "attendances");
        studentData.absence = SetAttendanceAverage(studentData.attendanceBools, "absence");
        studentData.totalDays = SetAttendanceAverage(studentData.attendanceBools, "totalDays");
        studentData.noteAverage = SetNoteAverage(studentData.notes, false);
        studentData.notePercentage = SetNoteAverage(studentData.notes, true);
    }

    public void OrderStudents(List<StudentData> studentData)
    {
        studentData.Sort((student1, student2) => student1.lastName.CompareTo(student2.lastName));
    }

    public void SetCourses()
    {
        foreach (StudentData item in LoadData.instance.data.students)
        {
            if (!coursesNames.Contains(item.course)) NewCourse(item.course);
            AddNewStudent(item);
        }
    }

    public void NewCourse(string course)
    {
        GameObject newCourse = GameObject.Instantiate(prefabCourse);
        newCourse.transform.SetParent(parent.transform);
        if (coursesList.Count > 0) newCourse.transform.localPosition = coursesList.Peek().transform.localPosition - new Vector3(0, distanceObjects, 0);
        else newCourse.transform.localPosition = originalPosition;
        print(newCourse.transform.localPosition);
        coursesList.Push(newCourse);
        coursesNames.Add(course);
        CourseController courseController = newCourse.GetComponent<CourseController>();
        courseController.nameCourse = course;
        courseController.nameText.text = course;
        print(newCourse.transform.localPosition);
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
