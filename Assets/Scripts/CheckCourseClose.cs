using UnityEngine;

public class CheckCourseClose : OnEnableSp
{
    public GameObject[] closeObjects;

    private void Awake()
    {
        eventOnEnable.AddListener(CheckCourse);
    }
    public void CheckCourse()
    {
        foreach (GameObject go in closeObjects)
        {
            go.SetActive(!CoursesManager.instance.actualCourse.dataCourse.closed);
        }
    }

    private void OnDestroy()
    {
        eventOnEnable.RemoveAllListeners();

    }
}
