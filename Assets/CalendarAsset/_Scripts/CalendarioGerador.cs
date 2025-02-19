using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;

// Clase para almacenar la información de cada día
[System.Serializable]
public class DiaInfo
{
    public int Dia;
    public int Mes;
    public int Año;
    public EstadoAsistencia Estado;
    public DateTime Fecha;

    public DiaInfo(int dia, int mes, int año, EstadoAsistencia estado)
    {
        Dia = dia;
        Mes = mes;
        Año = año;
        Estado = estado;
        Fecha = new DateTime(año, mes, dia);
    }
}
public class CalendarioGerador : MonoBehaviour
{
    private DateTime _dataCalendarioExibido;
    [SerializeField] private Dia[] _dias;
    [SerializeField] private TextMeshProUGUI[] _semanasText;
    [SerializeField] private TextMeshProUGUI _mesAnoTexto;
    private DateTimeFormatInfo _traducao;

    // Nueva variable para la fecha inicial
    [SerializeField] private string fechaInicialStr = "2025-01-01"; // Formato YYYY-MM-DD
    private DateTime fechaInicial;
    private DateTime fechaActual;

    private Dictionary<string, DiaInfo> _historialDias = new Dictionary<string, DiaInfo>();

    private void Start()
    {
        if (!DateTime.TryParse(fechaInicialStr, out fechaInicial))
        {
            Debug.LogError("Formato de fecha inicial inválido. Usando fecha actual.");
            fechaInicial = DateTime.Today;
        }

        fechaActual = DateTime.Today;
        InicializaCalendario();
        MarcarFaltasAutomaticas();
    }
    private void MarcarFaltasAutomaticas()
    {
        DateTime fechaIteracion = fechaInicial;

        while (fechaIteracion <= fechaActual)
        {
            string clave = GenerarClaveDia(fechaIteracion.Day, fechaIteracion.Month, fechaIteracion.Year);

            // Solo marca falta si:
            // 1. No es fin de semana
            // 2. No tiene un estado guardado
            // 3. La fecha es anterior o igual a la actual
            if (!EsFinDeSemana(fechaIteracion) &&
                !_historialDias.ContainsKey(clave) &&
                fechaIteracion <= fechaActual)
            {
                GuardarEstadoDia(fechaIteracion.Day, fechaIteracion.Month, fechaIteracion.Year, EstadoAsistencia.noSelected);
            }

            fechaIteracion = fechaIteracion.AddDays(1);
        }
    }
    private bool EsFinDeSemana(DateTime fecha)
    {
        return fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday;
    }
    // Método para generar la clave única para cada día
    private string GenerarClaveDia(int dia, int mes, int año)
    {
        return $"{dia}-{mes}-{año}";
    }

    // Método para guardar el estado de un día
    public void GuardarEstadoDia(int dia, int mes, int año, EstadoAsistencia estado)
    {
        string clave = GenerarClaveDia(dia, mes, año);
        if (_historialDias.ContainsKey(clave))
        {
            _historialDias[clave].Estado = estado;
        }
        else
        {
            _historialDias.Add(clave, new DiaInfo(dia, mes, año, estado));
        }
    }

    // Método para obtener el estado guardado de un día
    private EstadoAsistencia ObtenerEstadoDia(int dia, int mes, int año)
    {
        string clave = GenerarClaveDia(dia, mes, año);
        if (_historialDias.ContainsKey(clave))
        {
            return _historialDias[clave].Estado;
        }

        DateTime fechaConsultada = new DateTime(año, mes, dia);

        // Si la fecha es anterior o igual a la actual y posterior o igual a la inicial
        if (fechaConsultada <= fechaActual && fechaConsultada >= fechaInicial && !EsFinDeSemana(fechaConsultada))
        {
            return EstadoAsistencia.noSelected;
        }

        return EstadoAsistencia.SinClase;
    }

    private void InicializaCalendario()
    {
        _dataCalendarioExibido = DateTime.Today;
        _traducao = CultureInfo.GetCultureInfo("es-AR").DateTimeFormat;
        GerarSemanas();
        SetTextoMesAno();
        GerarDias();
    }
    private void GerarSemanas()
    {
        for (int i = 0; i < _semanasText.Length; i++)
        {
            _semanasText[i].text = _traducao.GetDayName((DayOfWeek)i)[0].ToString().ToUpper();
        }
    }

    private void SetTextoMesAno()
    {
        _mesAnoTexto.text = string.Format("{0} {1}", _traducao.GetAbbreviatedMonthName(_dataCalendarioExibido.Month).ToUpper(), _dataCalendarioExibido.Year.ToString());
    }

    private void GerarDias()
    {
        DateTime primeiroDia = new DateTime(_dataCalendarioExibido.Year, _dataCalendarioExibido.Month, 1);
        ReiniciarDias();

        for (int i = (int)primeiroDia.DayOfWeek, dia = 1; dia <= DateTime.DaysInMonth(_dataCalendarioExibido.Year, _dataCalendarioExibido.Month); i++, dia++)
        {
            _dias[i].SetDiaAtivo(true);
            _dias[i].AtualizarDiaTexto(dia.ToString());

            DateTime fechaDia = new DateTime(_dataCalendarioExibido.Year, _dataCalendarioExibido.Month, dia);
            bool esFuturo = fechaDia > fechaActual;
            bool esAnteriorAInicial = fechaDia < fechaInicial;

            int diaDaSemana = (i % 7);
            _dias[i].weekend = (diaDaSemana == 0 || diaDaSemana == 6);

            // Obtener el estado inicial para este día
            EstadoAsistencia estadoInicial = ObtenerEstadoDia(dia, _dataCalendarioExibido.Month, _dataCalendarioExibido.Year);

            _dias[i].EstablecerEstado(estadoInicial);
            _dias[i].ConfigurarDiaActual(dia, _dataCalendarioExibido.Month, _dataCalendarioExibido.Year, this);
            _dias[i].SetButton();

            // Deshabilitar días futuros y anteriores a la fecha inicial
            _dias[i].SetInteractable(!esFuturo && !esAnteriorAInicial);
        }
    }
    public void AlteraMes(int sentido)
    {
        _dataCalendarioExibido = _dataCalendarioExibido.AddMonths(sentido);
        GerarDias();
        SetTextoMesAno();
    }

    public void AlteraAno(int sentido)
    {
        _dataCalendarioExibido = _dataCalendarioExibido.AddYears(sentido);
        GerarDias();
        SetTextoMesAno();
    }

    private void ReiniciarDias()
    {
        for (int i = 0; i < _dias.Length; i++)
        {
            _dias[i].SetDiaAtivo(false);
        }
    }
}