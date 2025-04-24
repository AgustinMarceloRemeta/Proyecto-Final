using UnityEngine;

public class DeleteNoteButton : MonoBehaviour
{
   public Note note;
    public void DeleteNote()
    {
        // Aquí implementarías la lógica para eliminar la nota
        if (NoteManager.instance != null && NoteManager.instance.selectedStudent != null)
        {
            NoteManager.instance.selectedStudent.notes.Remove(note);
            NotesListManager.instance.SetNotes();

            // Opcional: Mostrar confirmación de eliminación
            // UIManager.instance.ShowMessage("Nota eliminada con éxito");
        }
    }
}
