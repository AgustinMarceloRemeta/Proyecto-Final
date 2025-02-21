using System.Globalization;
using System;
using UnityEngine;
using TMPro;

public class NoteManager : MonoBehaviour
{
    public StudentData selectedStudent;
    public TextMeshProUGUI referenceText, noteValueText, notesText, alertText;
    public static NoteManager instance;
    public GameObject notePanel;
    private void Awake()
    {
        instance = this;
    }
    public void AddNote()
    {
        Note note = new Note();
        if (string.IsNullOrWhiteSpace(referenceText.text)) //verifica si la referencia es valida
        {
            Alert.instance.StartAlert(AlertTexts.textInvalid, alertText);
            return;
        }
        string cleanValueText = noteValueText.text.Replace("\u200B", "");
        try //verifica si la nota es valida
        {
            note.value = int.Parse(cleanValueText.Trim(), CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
            Alert.instance.StartAlert(AlertTexts.valueInvalid, alertText);
            return;
        }
        note.referencia = referenceText.text;

        selectedStudent.notes.Add(note);
        UpdateText();
    }
    public void UpdateText()
    {
        notesText.text = "Notas: ";
        foreach (var item in selectedStudent.notes)
        {
            notesText.text += item.referencia + " " + item.value + " / ";
        }
    }
}
