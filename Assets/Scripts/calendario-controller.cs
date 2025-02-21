using UnityEngine;
using System.Collections.Generic;

public class CalendarioController : MonoBehaviour
{
    public static CalendarioController Instance { get; private set; }
    
    [SerializeField] private CalendarioGerador calendarioGerador;
    private Dictionary<string, DiaInfo> historialDias = new Dictionary<string, DiaInfo>();
    public StudentData student;
    public GameObject panel;
    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    // Función para establecer la fecha inicial
    public void SetInitialDate(string initialDate)
    {
        calendarioGerador.SetInitialDate(initialDate);
    }

    // Función para actualizar el historial de días
    public void UpdateHistorialDias(Dictionary<string, DiaInfo> newHistorial)
    {
        historialDias = new Dictionary<string, DiaInfo>(newHistorial);
        calendarioGerador.SetHistorialDias(historialDias);
        calendarioGerador.Inicialize();
    }

    // Función para obtener el historial actual
    public Dictionary<string, DiaInfo> GetHistorialDias()
    {
        return calendarioGerador.ReturnHistorialDias();
    }

    // Función para agregar un nuevo día al historial
    public void AddDiaToHistorial(DiaInfo nuevoDia)
    {
        string clave = $"{nuevoDia.Dia}-{nuevoDia.Mes}-{nuevoDia.Año}";
        if (historialDias.ContainsKey(clave))
        {
            historialDias[clave] = nuevoDia;
        }
        else
        {
            historialDias.Add(clave, nuevoDia);
        }
        calendarioGerador.SetHistorialDias(historialDias);
    }

   

    // Función para limpiar el historial
    public void ClearHistorial()
    {
        historialDias.Clear();
        calendarioGerador.SetHistorialDias(historialDias);
    }

    public void EndCalendar()
    {
        student.historialDiasList = DictionaryConverter.ConvertDictionaryToDiaInfoEntries(GetHistorialDias());
        ClearHistorial();
    }
}
