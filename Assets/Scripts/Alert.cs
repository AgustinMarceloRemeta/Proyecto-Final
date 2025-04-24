using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Alert : MonoBehaviour
{
  public static Alert instance;
    public TextMeshProUGUI textAlert;
    public bool alertActive = false;
    [SerializeField] int countRepeatAlert = 3;
    [SerializeField] float velocityAlert = 0.5f;
    private void Awake()
    {
        instance = this;
    }

    public void StartAlert(string message)
    {
        StopAllCoroutines();
        alertActive = true;
        textAlert.text = message;
        StartCoroutine(AlertCoroutine(message, textAlert, countRepeatAlert, textAlert.color));
    }

    public IEnumerator AlertCoroutine(string message, TextMeshProUGUI text, int countBlinks,Color colorText) 
    {
        if (countBlinks >0) 
        {
            text.transform.parent.gameObject.SetActive(true);
            yield return new WaitForSeconds(velocityAlert);
            text.transform.parent.gameObject.SetActive(false);
            yield return new WaitForSeconds(velocityAlert);
            StartCoroutine(AlertCoroutine(message,text,countBlinks-1, colorText));
        }   
        else
        {
            text.transform.parent.gameObject.SetActive(false);

            alertActive = false;
            yield return null;
        }
    }
}