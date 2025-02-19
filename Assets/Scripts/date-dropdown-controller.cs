using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

public class DateDropdownController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown yearDropdown;
    [SerializeField] private TMP_Dropdown monthDropdown;
    [SerializeField] private TMP_Dropdown dayDropdown;

    private void Start()
    {
        InitializeYearDropdown();
        InitializeMonthDropdown();

        // Añadir listeners para cuando cambien los valores
        monthDropdown.onValueChanged.AddListener(delegate { UpdateDayDropdown(); });
        yearDropdown.onValueChanged.AddListener(delegate { UpdateDayDropdown(); });

        // Inicializar con la fecha actual
        SetCurrentDate();
    }

    private void SetCurrentDate()
    {
        DateTime now = DateTime.Now;

        // Establecer año actual
        int yearIndex = now.Year - 2020; // Calcular el índice basado en el año inicial (2020)
        if (yearIndex >= 0 && yearIndex <= 10) // Asegurarse que está en el rango 2020-2030
        {
            yearDropdown.value = yearIndex;
        }

        // Establecer mes actual (restamos 1 porque los índices empiezan en 0)
        monthDropdown.value = now.Month - 1;

        // Actualizar días basado en el mes actual
        UpdateDayDropdown();

        // Establecer día actual (restamos 1 porque los índices empiezan en 0)
        dayDropdown.value = now.Day - 1;
    }

    private void InitializeYearDropdown()
    {
        List<string> years = new List<string>();
        for (int year = 2020; year <= 2030; year++)
        {
            years.Add(year.ToString());
        }
        yearDropdown.ClearOptions();
        yearDropdown.AddOptions(years);
    }

    private void InitializeMonthDropdown()
    {
        List<string> months = new List<string>();
        for (int month = 1; month <= 12; month++)
        {
            months.Add(month.ToString("00"));
        }
        monthDropdown.ClearOptions();
        monthDropdown.AddOptions(months);
    }

    private void UpdateDayDropdown()
    {
        int selectedMonth = monthDropdown.value + 1; // +1 porque el índice empieza en 0
        int selectedYear = 2020 + yearDropdown.value;

        // Obtener el último día del mes seleccionado
        int daysInMonth = GetDaysInMonth(selectedMonth, selectedYear);

        List<string> days = new List<string>();
        for (int day = 1; day <= daysInMonth; day++)
        {
            days.Add(day.ToString("00"));
        }

        // Guardar el valor actual antes de limpiar
        int previousValue = dayDropdown.value;

        dayDropdown.ClearOptions();
        dayDropdown.AddOptions(days);

        // Restaurar el valor anterior si es válido
        if (previousValue < days.Count)
        {
            dayDropdown.value = previousValue;
        }
        else
        {
            dayDropdown.value = 0;
        }
    }

    private int GetDaysInMonth(int month, int year)
    {
        switch (month)
        {
            case 4:
            case 6:
            case 9:
            case 11:
                return 30;
            case 2:
                // Verificar si es año bisiesto
                if ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0))
                    return 29;
                return 28;
            default:
                return 31;
        }
    }

    // Método opcional para obtener la fecha seleccionada como string
    public string GetSelectedDate()
    {
        int year = 2020 + yearDropdown.value;
        int month = monthDropdown.value + 1;
        int day = dayDropdown.value + 1;
        return $"{year}-{month:00}-{day:00}";
    }
}