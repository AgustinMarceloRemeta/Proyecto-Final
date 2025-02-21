using UnityEngine;

public class ButtonDelete : ButtonFunction
{
    public override void PlayButton()
    {
        LoadData.instance.data.students.Remove(student);
        StudentsListManager.instance.SetStudents();
    }
}
