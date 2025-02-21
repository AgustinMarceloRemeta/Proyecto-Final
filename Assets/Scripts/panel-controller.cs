using UnityEngine;
using System.Collections.Generic;

public class PanelController : MonoBehaviour
{
    // Stack para guardar el historial de paneles
    private Stack<GameObject> panelHistory = new Stack<GameObject>();
    
    // Panel actualmente activo
    private GameObject currentPanel;

    public static PanelController instance;

    private void Awake()
    {
        instance = this;
    }
    // Método para activar un nuevo panel
    public void ShowPanel(GameObject panel)
    {
        // Si hay un panel activo, lo desactivamos y lo guardamos en el historial
        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
            panelHistory.Push(currentPanel);
        }

        // Activamos el nuevo panel
        panel.SetActive(true);
        currentPanel = panel;
    }

    // Método para volver al panel anterior
    public void ReturnToPreviousPanel()
    {
        // Verificamos si hay paneles en el historial
        if (panelHistory.Count > 0)
        {
            // Desactivamos el panel actual
            if (currentPanel != null)
            {
                currentPanel.SetActive(false);
            }

            // Recuperamos y activamos el panel anterior
            currentPanel = panelHistory.Pop();
            currentPanel.SetActive(true);
        }
        else
        {
            Debug.Log("No hay paneles anteriores en el historial");
        }
    }

    // Método para limpiar el historial
    public void ClearHistory()
    {
        panelHistory.Clear();
    }

    // Método para obtener la cantidad de paneles en el historial
    public int GetHistoryCount()
    {
        return panelHistory.Count;
    }
}
