using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum EstadoAsistencia
{
    Asistio,    // Verde
    NoAsistio,  // Rojo
    SinClase    // Gris
}

public class Dia : MonoBehaviour
{
    private TextMeshProUGUI _diaTexto;
    private Button button;
    public bool selected = false;
    public bool weekend = false;
    private EstadoAsistencia estadoActual = EstadoAsistencia.SinClase;

    private void Awake()
    {
        _diaTexto = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponentInChildren<Button>();
    }

    public void SetButton()
    {
        if (weekend)
        {
            estadoActual = EstadoAsistencia.SinClase;
            ActualizarColor();
        }
        button.onClick.AddListener(() => OnClickButton());
    }

    public void SetDiaAtivo(bool ativo)
    {
        _diaTexto.gameObject.SetActive(ativo);
    }

    public void AtualizarDiaTexto(string novoDia)
    {
        _diaTexto.text = novoDia;
    }

    public void OnClickButton()
    {
        // Agregar debug para ver el flujo
        Debug.Log($"Click - Estado Actual: {estadoActual}, Selected: {selected}");

        if (!selected)
        {
            selected = true;
            estadoActual = EstadoAsistencia.Asistio;
            ActualizarColor();
        }
        else
        {
            // Si está seleccionado, cambia al siguiente estado
            CambiarEstado();
        }

        Debug.Log($"Después del cambio - Estado Actual: {estadoActual}, Selected: {selected}");
    }

    private void CambiarEstado()
    {
        switch (estadoActual)
        {
            case EstadoAsistencia.Asistio:
                estadoActual = EstadoAsistencia.NoAsistio;
                break;
            case EstadoAsistencia.NoAsistio:
                estadoActual = EstadoAsistencia.SinClase;
                break;
            case EstadoAsistencia.SinClase:
                estadoActual = EstadoAsistencia.Asistio;
                break;
        }

        ActualizarColor();
        // No deseleccionamos aquí para permitir múltiples cambios
    }

    private void ActualizarColor()
    {
        switch (estadoActual)
        {
            case EstadoAsistencia.Asistio:
                _diaTexto.color = Color.green;
                break;
            case EstadoAsistencia.NoAsistio:
                _diaTexto.color = Color.red;
                break;
            case EstadoAsistencia.SinClase:
                _diaTexto.color = Color.grey;
                break;
        }
    }
}