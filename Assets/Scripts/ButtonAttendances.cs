using UnityEngine;

public class ButtonAttendances : ButtonFunction
{
    public override void PlayButton()
    {
        PanelController.instance.ShowPanel(CalendarioController.Instance.panel);
        //CalendarioController.Instance.SetInitialDate(CoursesManager.instance.GetCourse(student.course).initialDate);
        CalendarioController.Instance.student=student;
        CalendarioController.Instance.UpdateHistorialDias(DictionaryConverter.ConvertDiaInfoEntriesToDictionary(student.historialDiasList));

    }
}
