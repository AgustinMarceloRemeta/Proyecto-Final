using UnityEngine;

public class DeleteNoteButton : MonoBehaviour
{
   public Note note;
    public void DeleteNote()
    {
        // Aqu� implementar�as la l�gica para eliminar la nota
        if (NoteManager.instance != null && NoteManager.instance.selectedStudent != null)
        {
            NoteManager.instance.selectedStudent.notes.Remove(note);
            NotesListManager.instance.SetNotes();

            // Opcional: Mostrar confirmaci�n de eliminaci�n
            // UIManager.instance.ShowMessage("Nota eliminada con �xito");
        }
    }
}
