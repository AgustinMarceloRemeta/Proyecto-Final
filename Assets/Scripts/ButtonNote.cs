using UnityEngine;

public class ButtonNote : ButtonFunction
{
    public override void PlayButton()
    {
        NoteManager.instance.selectedStudent = student;
        NoteManager.instance.UpdateText();
        PanelController.instance.ShowPanel(NoteManager.instance.notePanel);
    }
}
