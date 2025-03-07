using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{

    public void clearText(TMP_InputField text)
    {
        text.text = string.Empty;
    }
}
