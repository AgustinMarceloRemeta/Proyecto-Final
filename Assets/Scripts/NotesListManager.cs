using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotesListManager : MonoBehaviour
{
    public static NotesListManager instance;

    [Header("Prefabs")]
    [SerializeField] private TextMeshProUGUI textPrefab;
    [SerializeField] private Button delButton;

    [Header("Configuration")]
    [SerializeField] private float verticalSpacing = 100f;
    [SerializeField] private float initialTopOffset = 20f; // Espacio inicial desde arriba

    [Header("Containers")]
    [SerializeField] private RectTransform contentRectTransform;

    [SerializeField] private RectTransform referenceContainer;
    [SerializeField] private RectTransform scoreContainer;
    [SerializeField] private RectTransform delButtonContainer;

    private List<TextMeshProUGUI> referenceTexts = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> scoreTexts = new List<TextMeshProUGUI>();
    private List<Button> delButtons = new List<Button>();

    private void Awake()
    {
        instance = this;
    }

    public void SetNotes()
    {
        if (NoteManager.instance.selectedStudent == null) return;
        SetList(NoteManager.instance.selectedStudent.notes);
    }

    private void ClearAllTexts()
    {
        ClearTextList(referenceTexts);
        ClearTextList(scoreTexts);
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

    public void SetList(List<Note> notes)
    {
        ClearAllTexts();
        ClearAllButtons();

        for (int i = 0; i < notes.Count; i++)
        {
            Note note = notes[i];
            float yOffset = -verticalSpacing * i;

            // Crear textos para cada columna
            CreateText(note.referencia, referenceContainer, yOffset, referenceTexts);
            CreateText(note.value.ToString(), scoreContainer, yOffset, scoreTexts);
            
            // Crear botones
            CreateButton(delButtonContainer, yOffset, delButtons, delButton, note);
        }

        float totalHeight = initialTopOffset + (verticalSpacing * notes.Count);

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

    private void CreateButton(RectTransform container, float yOffset, List<Button> buttonList, Button buttonPrefab, Note note)
    {
        Button newButton = Instantiate(buttonPrefab, container);
        newButton.GetComponent<DeleteNoteButton>().note = note;

        // Mantener la posición X del container y solo modificar la Y
        RectTransform rectTransform = newButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, yOffset);

        // Copiar los valores de anchor y pivot del container
        rectTransform.anchorMin = container.anchorMin;
        rectTransform.anchorMax = container.anchorMax;
        rectTransform.pivot = container.pivot;

        buttonList.Add(newButton);
    }

    private void ClearAllButtons()
    {
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
