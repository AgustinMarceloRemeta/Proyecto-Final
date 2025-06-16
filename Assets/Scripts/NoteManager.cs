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
    public TextMeshProUGUI nameStudent, nameStudent2, nameStudent3;
    private void Awake()
    {
        instance = this;
    }
    public void AddNote()
    {
        Note note = new Note();
        if (string.IsNullOrWhiteSpace(referenceText.text)) //verifica si la referencia es valida
        {
            Alert.instance.StartAlert(AlertTexts.textInvalid);
            return;
        }
        string cleanValueText = noteValueText.text.Replace("\u200B", "");
        try //verifica si la nota es valida
        {
            note.value = int.Parse(cleanValueText.Trim(), CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
            Alert.instance.StartAlert(AlertTexts.valueInvalid);
            return;
        }

        if(note.value < 0)
        {
            Alert.instance.StartAlert(AlertTexts.valueInvalid);
            return;
        }
        else if(note.value > NoteManager.instance.selectedStudent.course.noteMax)
        {
            Alert.instance.StartAlert(AlertTexts.valueInvalid);
            return;
        }
        note.referencia = referenceText.text;
        if(note.referencia == string.Empty)
        {
            Alert.instance.StartAlert(AlertTexts.emptyText);
            return;
        }

        selectedStudent.notes.Add(note);
        RewardController.instance.AddReward(Reward.firtsNote);
        Alert.instance.StartAlert(AlertTexts.correctNote);
    }
    public void UpdateText()
    {
        nameStudent.text = selectedStudent.lastName + " " + selectedStudent.name; 
        nameStudent2.text = selectedStudent.lastName + " " + selectedStudent.name; 
        nameStudent3.text = selectedStudent.lastName + " " + selectedStudent.name; 
        /*
        notesText.text = "Notas: ";
        foreach (var item in selectedStudent.notes)
        {
            notesText.text += item.referencia + " " + item.value + " / ";
        }*/
    }

    public void AddAttendances()
    {
        PanelController.instance.ShowPanel(CalendarioController.Instance.panel);
        CalendarioController.Instance.SetInitialDate(CoursesManager.instance.GetCourse(selectedStudent.course.courseName).initialDate);
        CalendarioController.Instance.student = selectedStudent;
        CalendarioController.Instance.UpdateHistorialDias(DictionaryConverter.ConvertDiaInfoEntriesToDictionary(selectedStudent.historialDiasList));
    }
}
