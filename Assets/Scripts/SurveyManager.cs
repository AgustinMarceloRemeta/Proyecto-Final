using System;
using UnityEngine;
using UnityEngine.UI;

public class SurveyManager : MonoBehaviour
{
    [SerializeField] Button surveyButton;
    [SerializeField] string url;
    DateTime minLimitDate = new DateTime(2025, 2, 15); 
    DateTime maxLimitDate = new DateTime(2025, 3, 15); 

    private void Awake()
    {
        if (DateTime.Compare(DateTime.Today, minLimitDate) >= 0 && DateTime.Compare(DateTime.Today, maxLimitDate) <= 0) surveyButton.gameObject.SetActive(true);
        else surveyButton.gameObject.SetActive(false);
    }

    public void OpenSurvey()
    {
        Application.OpenURL(url);
    }
}
