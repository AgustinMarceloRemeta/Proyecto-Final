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

    public DiaInfo(int dia, int mes, int año, EstadoAsistencia estado)
    {
        Dia = dia;
        Mes = mes;
        Año = año;
        Estado = estado;
    }
}

public class CalendarioGerador : MonoBehaviour
{
    private DateTime _dataCalendarioExibido;
    [SerializeField] private Dia[] _dias;
    [SerializeField] private TextMeshProUGUI[] _semanasText;
    [SerializeField] private TextMeshProUGUI _mesAnoTexto;
    private DateTimeFormatInfo _traducao;

    // Diccionario para almacenar el historial de estados
    private Dictionary<string, DiaInfo> _historialDias = new Dictionary<string, DiaInfo>();

    private void Start()
    {
        InicializaCalendario();
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

            int diaDaSemana = (i % 7);
            _dias[i].weekend = (diaDaSemana == 0 || diaDaSemana == 6);

            // Recuperar el estado guardado para este día
            string clave = GenerarClaveDia(dia, _dataCalendarioExibido.Month, _dataCalendarioExibido.Year);
            EstadoAsistencia estadoInicial;

            if (_historialDias.ContainsKey(clave))
            {
                // Si ya tiene un estado guardado, usar ese
                estadoInicial = _historialDias[clave].Estado;
            }
            else
            {
                // Si es fin de semana, iniciar en gris (SinClase)
                // Si no, iniciar en verde (Asistio)
                estadoInicial = _dias[i].weekend ? EstadoAsistencia.SinClase : EstadoAsistencia.noSelected;
            }

            _dias[i].EstablecerEstado(estadoInicial);
            _dias[i].ConfigurarDiaActual(dia, _dataCalendarioExibido.Month, _dataCalendarioExibido.Year, this);
            _dias[i].SetButton();
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