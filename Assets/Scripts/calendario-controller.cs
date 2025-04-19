using UnityEngine;
using System.Collections.Generic;
using System;

public class CalendarioController : MonoBehaviour
{
    public static CalendarioController Instance { get; private set; }

    [SerializeField] private CalendarioGerador calendarioGerador;
    private Dictionary<string, DiaInfo> historialDias = new Dictionary<string, DiaInfo>();
    public StudentData student;
    public GameObject panel;
    private DateTime fechaInicial;

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

        // Parsear la fecha inicial
        if (DateTime.TryParse(initialDate, out fechaInicial))
        {
            // Limpia cualquier registro previo a la fecha inicial
            LimpiarDiasPreviosAFechaInicial();
        }
        else
        {
            Debug.LogError("Formato de fecha inicial inválido.");
        }
    }

    // Nueva función para limpiar días previos a la fecha inicial
    private void LimpiarDiasPreviosAFechaInicial()
    {
        List<string> clavesAEliminar = new List<string>();

        foreach (var entry in historialDias)
        {
            DiaInfo diaInfo = entry.Value;
            DateTime fechaDia = new DateTime(diaInfo.Año, diaInfo.Mes, diaInfo.Dia);

            // Si la fecha es anterior a la fecha inicial, marcar para eliminar
            if (fechaDia < fechaInicial)
            {
                clavesAEliminar.Add(entry.Key);
            }
        }

        // Eliminar las entradas marcadas
        foreach (string clave in clavesAEliminar)
        {
            historialDias.Remove(clave);
        }

        // Actualizar el calendario con el historial limpio
        calendarioGerador.SetHistorialDias(historialDias);
    }

    // Función para actualizar el historial de días
    public void UpdateHistorialDias(Dictionary<string, DiaInfo> newHistorial)
    {
        historialDias = new Dictionary<string, DiaInfo>(newHistorial);

        // Verificar si la fecha inicial está configurada
        if (fechaInicial != default(DateTime))
        {
            LimpiarDiasPreviosAFechaInicial();
        }

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

        // Verificar si la fecha del día es anterior a la fecha inicial
        DateTime fechaDia = new DateTime(nuevoDia.Año, nuevoDia.Mes, nuevoDia.Dia);
        if (fechaInicial != default(DateTime) && fechaDia < fechaInicial)
        {
            // No agregar días anteriores a la fecha inicial
            return;
        }

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

    public void AddNotes()
    {
        NoteManager.instance.selectedStudent = student;
        NoteManager.instance.UpdateText();
        PanelController.instance.ShowPanelWithoutSaving(NoteManager.instance.notePanel);
    }
}