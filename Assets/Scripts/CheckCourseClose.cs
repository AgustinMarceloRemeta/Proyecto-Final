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
            if(CoursesManager.instance.actualCourse!= null) go.SetActive(!CoursesManager.instance.actualCourse.dataCourse.closed);
            else go.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        eventOnEnable.RemoveAllListeners();

    }
}
