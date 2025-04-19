using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{

    public void ClearAllText()
    {
        foreach (var item in FindObjectsOfType<TMP_InputField>())
        {
            clearText(item);
        }
    }

    public void clearText(TMP_InputField text)
    {
        text.text = string.Empty;
    }

    public void clearAlertText(TextMeshProUGUI text)
    {
        text.text = string.Empty;
    }
}
