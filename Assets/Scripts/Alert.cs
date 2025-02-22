using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Alert : MonoBehaviour
{
  public static Alert instance;
    public TextMeshProUGUI textAlert;
    public bool alertActive = false;
    private void Awake()
    {
        instance = this;
    }

    public void StartAlert(string message)
    {
        alertActive = true;
        textAlert.text = message;
        StartCoroutine(AlertCoroutine(message, textAlert, 6, textAlert.color));
    }

    public IEnumerator AlertCoroutine(string message, TextMeshProUGUI text, int countBlinks,Color colorText) 
    {
        if (countBlinks >0) 
        {
            text.color = Color.red + new Color(0, 0, 0, 1);
            yield return new WaitForSeconds(0.5f);
            text.color = new Color(0, 0, 0, 0);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(AlertCoroutine(message,text,countBlinks-1, colorText));
        }   
        else
        {
            text.color = colorText;
            alertActive = false;
            yield return null;
        }
    }
}