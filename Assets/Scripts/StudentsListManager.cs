using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StudentsListManager : MonoBehaviour
{
    public static StudentsListManager instance;

    [Header("Prefabs")]
    [SerializeField] private TextMeshProUGUI textPrefab;
    [SerializeField] private Button noteButton,attendanceButton,delButton;

    [Header("Configuration")]
    [SerializeField] private float verticalSpacing = 100f;
    [SerializeField] private float initialTopOffset = 20f; // Espacio inicial desde arriba


    [Header("Containers")]
    [SerializeField] private RectTransform contentRectTransform;
    [SerializeField] private RectTransform lastNameContainer;
    [SerializeField] private RectTransform nameContainer;
    [SerializeField] private RectTransform attendancesContainer;
    [SerializeField] private RectTransform noteContainer;
    [SerializeField] private RectTransform noteButtonContainer;
    [SerializeField] private RectTransform attendanceButtonContainer;
    [SerializeField] private RectTransform delButtonContainer;

    private List<TextMeshProUGUI> lastNameTexts = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> nameTexts = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> attendanceTexts = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> noteTexts = new List<TextMeshProUGUI>();
    private List<Button> noteButtons = new List<Button>();
    private List<Button> attendanceButtons = new List<Button>();
    private List<Button> delButtons = new List<Button>();

    private void Awake()
    {
        instance = this;
    }

    public void SetStudents()
    {
        if (CoursesManager.instance.actualCourse == null) SetList(LoadData.instance.data.students);
        else SetList(CoursesManager.instance.actualCourse.students);
    }

    private void ClearAllTexts()
    {
        ClearTextList(lastNameTexts);
        ClearTextList(nameTexts);
        ClearTextList(attendanceTexts);
        ClearTextList(noteTexts);
    }

    private void ClearTextList(List<TextMeshProUGUI> texts)
    {
        foreach (var text in texts)
        {
            if (text != null)
                Destroy(text.gameObject);
        }
        texts.Clear();
    }

    public void SetList(List<StudentData> students)
    {
        ClearAllTexts();
        ClearAllButtons();

        for (int i = 0; i < students.Count; i++)
        {
            StudentData student = students[i];
            float yOffset = -verticalSpacing * i;
            student.absence = ConvertDiaInfoToAbtense(student.historialDiasList);
            student.attendances = ConvertDiaInfoToAttendances(student.historialDiasList);
            DataManager.instance.SetAverages(student, CoursesManager.instance.GetCourse(student.course).noteMax);
            // Usar la posici�n de los containers como punto de inicio
            CreateText(student.lastName, lastNameContainer, yOffset, lastNameTexts);
            CreateText(student.name, nameContainer, yOffset, nameTexts);
            CreateText($"{student.attendancePercentage}%", attendancesContainer, yOffset, attendanceTexts);
            CreateText(student.noteAverage.ToString(), noteContainer, yOffset, noteTexts);
            CreateButton(noteButtonContainer, yOffset, noteButtons, noteButton, student);
            CreateButton(attendanceButtonContainer, yOffset, attendanceButtons, attendanceButton, student);
            CreateButton(delButtonContainer, yOffset, delButtons, delButton, student);

        }
        float totalHeight = initialTopOffset + (verticalSpacing * students.Count);

        // Ajustar el tama�o del content
        Vector2 currentSize = contentRectTransform.sizeDelta;
        contentRectTransform.sizeDelta = new Vector2(currentSize.x, totalHeight + 50);
    }

    private void CreateText(string content, RectTransform container, float yOffset, List<TextMeshProUGUI> textList)
    {
        TextMeshProUGUI newText = Instantiate(textPrefab, container);
        newText.text = content;

        // Mantener la posici�n X del container y solo modificar la Y
        RectTransform rectTransform = newText.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, yOffset);

        // Copiar los valores de anchor y pivot del container
        rectTransform.anchorMin = container.anchorMin;
        rectTransform.anchorMax = container.anchorMax;
        rectTransform.pivot = container.pivot;

        textList.Add(newText);
    }
    private void CreateButton(RectTransform container, float yOffset, List<Button> buttonList,Button buttonPrefab,StudentData student)
    {
        Button newButton = Instantiate(buttonPrefab, container);
        newButton.GetComponent<ButtonFunction>().student = student;

        // Mantener la posici�n X del container y solo modificar la Y
        RectTransform rectTransform = newButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, yOffset);

        // Copiar los valores de anchor y pivot del container
        rectTransform.anchorMin = container.anchorMin;
        rectTransform.anchorMax = container.anchorMax;
        rectTransform.pivot = container.pivot;

        buttonList.Add(newButton);
    }

    public static float ConvertDiaInfoToAbtense(List<DiaInfoEntry> entries)
    {
        float value = 0;
        foreach (DiaInfoEntry entry in entries)
        {
            if (entry.value.Estado == EstadoAsistencia.NoAsistio) value++;
        }
        return value;
    } 
    public static float ConvertDiaInfoToAttendances(List<DiaInfoEntry> entries)
    {
        float value = 0;
        foreach (DiaInfoEntry entry in entries)
        {
            if (entry.value.Estado == EstadoAsistencia.Asistio) value++;
        }
        return value;
    }
    private void ClearAllButtons()
    {
        ClearButtonList(noteButtons);
        ClearButtonList(attendanceButtons);
        ClearButtonList(delButtons);
    }

    private void ClearButtonList(List<Button> buttons)
    {
        foreach (var button in buttons)
        {
            if (button != null)
                Destroy(button.gameObject);
        }
        buttons.Clear();
    }
}