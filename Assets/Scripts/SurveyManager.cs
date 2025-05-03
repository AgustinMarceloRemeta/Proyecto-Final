using System;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections.Generic;
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;

public class SurveyManager : MonoBehaviour
{
    [SerializeField] Button surveyButton;
    [SerializeField] GameObject loadingIndicator;

    // Keys para Remote Config
    private const string MIN_DATE_KEY = "survey_min_date";
    private const string MAX_DATE_KEY = "survey_max_date";
    private const string SURVEY_URL_KEY = "survey_url";

    // Valores por defecto (como respaldo si no se puede conectar)
    private DateTime defaultMinDate = new DateTime(2025, 2, 15);
    private DateTime defaultMaxDate = new DateTime(2025, 3, 15);
    private string defaultUrl = "https://tusindicado.com/encuesta";

    private string surveyUrl;
    SaveSurveyState saveSurveyState;
    public static SurveyManager instance;

    private void Awake()
    {
        saveSurveyState = GetComponent<SaveSurveyState>();
        instance = this;
    }

    public async void StartSurvery()
    {
        // Desactivar el bot�n y mostrar cargando hasta verificar fechas
        surveyButton.gameObject.SetActive(false);
        if (loadingIndicator != null) loadingIndicator.SetActive(true);

        // Inicializar y cargar los valores remotos
        await InitializeRemoteConfig();

        // Verificar fechas y actualizar UI
        CheckDateAndUpdateButton();

        // Ocultar indicador de carga
        if (loadingIndicator != null) loadingIndicator.SetActive(false);
    }

    private async Task InitializeRemoteConfig()
    {
        try
        {
            // Verificar y arreglar dependencias de Firebase primero
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus != DependencyStatus.Available)
            {
                Debug.LogError($"No se pudieron resolver las dependencias: {dependencyStatus}");
                return;
            }

            // Configurar los valores predeterminados
            var defaults = new Dictionary<string, object>
            {
                { MIN_DATE_KEY, defaultMinDate.ToString("yyyy-MM-dd") },
                { MAX_DATE_KEY, defaultMaxDate.ToString("yyyy-MM-dd") },
                { SURVEY_URL_KEY, defaultUrl }
            };

            // Establecer tiempo de cach� (bajo para desarrollo, m�s alto para producci�n)
            var configSettings = new ConfigSettings();
            configSettings.MinimumFetchIntervalInMilliseconds = 3600 * 1000; // 1 hora en producci�n
            await FirebaseRemoteConfig.DefaultInstance.SetConfigSettingsAsync(configSettings);

            // Establecer valores predeterminados
            await FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);

            // Intentar obtener los valores m�s recientes
            await FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync();

            Debug.Log("Firebase Remote Config inicializado correctamente");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al inicializar Remote Config: {ex.Message}");
            // Usaremos los valores por defecto en caso de error
        }
    }

    private async void CheckDateAndUpdateButton()
    {
        try
        {
            // Obtener valores de Remote Config
            string minDateStr = FirebaseRemoteConfig.DefaultInstance.GetValue(MIN_DATE_KEY).StringValue;
            string maxDateStr = FirebaseRemoteConfig.DefaultInstance.GetValue(MAX_DATE_KEY).StringValue;
            surveyUrl = FirebaseRemoteConfig.DefaultInstance.GetValue(SURVEY_URL_KEY).StringValue;

            // Parsear fechas
            DateTime minLimitDate = DateTime.Parse(minDateStr);
            DateTime maxLimitDate = DateTime.Parse(maxDateStr);

            Debug.Log($"Fechas de encuesta cargadas: Min={minDateStr}, Max={maxDateStr}");

            bool saveData = await saveSurveyState.ReturnData();

            // Verificar si la fecha actual est� dentro del rango
            if (DateTime.Compare(DateTime.Today, minLimitDate) >= 0 &&
                DateTime.Compare(DateTime.Today, maxLimitDate) <= 0 && !saveData) 
            {
                surveyButton.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                surveyButton.transform.parent.gameObject.SetActive(false);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al verificar fechas: {ex.Message}");

            // En caso de error, usar valores predeterminados
            if (DateTime.Compare(DateTime.Today, defaultMinDate) >= 0 &&
                DateTime.Compare(DateTime.Today, defaultMaxDate) <= 0)
            {
                surveyButton.gameObject.SetActive(true);
                surveyUrl = defaultUrl;
            }
            else
            {
                surveyButton.gameObject.SetActive(false);
            }
        }
    }

    public void OpenSurvey()
    {
        Application.OpenURL(surveyUrl);
    }

    // M�todo opcional para forzar recarga de configuraci�n (�til para testing)
    public async void RefreshConfig()
    {
        if (loadingIndicator != null) loadingIndicator.SetActive(true);
        await InitializeRemoteConfig();
        CheckDateAndUpdateButton();
        if (loadingIndicator != null) loadingIndicator.SetActive(false);
    }
}