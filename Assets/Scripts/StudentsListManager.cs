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

        for (int i = 0; i < students.Count; i++)
        {
            StudentData student = students[i];
            float yOffset = -verticalSpacing * i;

            // Usar la posición de los containers como punto de inicio
            CreateText(student.lastName, lastNameContainer, yOffset, lastNameTexts);
            CreateText(student.name, nameContainer, yOffset, nameTexts);
            CreateText($"{student.attendancePercentage}%", attendancesContainer, yOffset, attendanceTexts);
            CreateText(student.noteAverage.ToString(), noteContainer, yOffset, noteTexts);
            CreateButton(noteButtonContainer, yOffset, noteButtons, noteButton);
            CreateButton(attendanceButtonContainer, yOffset, attendanceButtons, attendanceButton);
            CreateButton(noteButtonContainer, yOffset, delButtons, delButton);

        }
        float totalHeight = initialTopOffset + (verticalSpacing * students.Count);

        // Ajustar el tamaño del content
        Vector2 currentSize = contentRectTransform.sizeDelta;
        contentRectTransform.sizeDelta = new Vector2(currentSize.x, totalHeight + 50);
    }

    private void CreateText(string content, RectTransform container, float yOffset, List<TextMeshProUGUI> textList)
    {
        TextMeshProUGUI newText = Instantiate(textPrefab, container);
        newText.text = content;

        // Mantener la posición X del container y solo modificar la Y
        RectTransform rectTransform = newText.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, yOffset);

        // Copiar los valores de anchor y pivot del container
        rectTransform.anchorMin = container.anchorMin;
        rectTransform.anchorMax = container.anchorMax;
        rectTransform.pivot = container.pivot;

        textList.Add(newText);
    }
    private void CreateButton(RectTransform container, float yOffset, List<Button> buttonList,Button buttonPrefab)
    {
        Button newButton = Instantiate(buttonPrefab, container);

        // Mantener la posición X del container y solo modificar la Y
        RectTransform rectTransform = newButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, yOffset);

        // Copiar los valores de anchor y pivot del container
        rectTransform.anchorMin = container.anchorMin;
        rectTransform.anchorMax = container.anchorMax;
        rectTransform.pivot = container.pivot;

        buttonList.Add(newButton);
    }
}