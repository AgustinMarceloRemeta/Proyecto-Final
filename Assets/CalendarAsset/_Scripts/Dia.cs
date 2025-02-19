using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum EstadoAsistencia
{
    Asistio,    // Verde
    NoAsistio,  // Rojo
    SinClase,    // Gris
    noSelected
}

public class Dia : MonoBehaviour
{
    private TextMeshProUGUI _diaTexto;
    private Button button;
    public bool weekend = false;
    private EstadoAsistencia estadoActual = EstadoAsistencia.noSelected;
    private int diaActual;
    private int mesActual;
    private int añoActual;
    private CalendarioGerador calendario;

    private void Awake()
    {
        _diaTexto = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponentInChildren<Button>();
        // Removido el estado inicial fijo
    }

    public void ConfigurarDiaActual(int dia, int mes, int año, CalendarioGerador cal)
    {
        diaActual = dia;
        mesActual = mes;
        añoActual = año;
        calendario = cal;
    }

    public void EstablecerEstado(EstadoAsistencia estado)
    {
        estadoActual = estado;
        ActualizarColor();
    }

    public void SetButton()
    {
        // Limpiar listeners anteriores para evitar duplicados
        button.onClick.RemoveAllListeners();

        if (weekend)
        {
            estadoActual = EstadoAsistencia.SinClase;
            ActualizarColor();
        }
        button.onClick.AddListener(OnClickButton);
    }

    public void SetDiaAtivo(bool ativo)
    {
        _diaTexto.gameObject.SetActive(ativo);
        button.interactable = ativo;
    }

    public void AtualizarDiaTexto(string novoDia)
    {
        _diaTexto.text = novoDia;
    }

    public void OnClickButton()
    {
        if (weekend) return; // No permitir cambios en días de fin de semana

        if (estadoActual == EstadoAsistencia.noSelected)
        {
            estadoActual = EstadoAsistencia.Asistio;
        }
        else
        {
            CambiarEstado();
        }

        ActualizarColor();
        calendario.GuardarEstadoDia(diaActual, mesActual, añoActual, estadoActual);
    }

    private void CambiarEstado()
    {
        estadoActual = estadoActual switch
        {
            EstadoAsistencia.Asistio => EstadoAsistencia.NoAsistio,
            EstadoAsistencia.NoAsistio => EstadoAsistencia.SinClase,
            EstadoAsistencia.SinClase => EstadoAsistencia.Asistio,
            _ => EstadoAsistencia.Asistio
        };
    }

    private void ActualizarColor()
    {
        _diaTexto.color = estadoActual switch
        {
            EstadoAsistencia.Asistio => Color.green,
            EstadoAsistencia.NoAsistio => Color.red,
            EstadoAsistencia.SinClase => Color.grey,
            EstadoAsistencia.noSelected => Color.white,
            _ => Color.white
        };
    }
}