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

            // Remover aparición anterior del panel si existe en el historial
            if (panelHistory.Contains(panel))
            {
                Stack<GameObject> tempStack = new Stack<GameObject>();

                // Sacar elementos hasta encontrar el panel duplicado
                while (panelHistory.Count > 0)
                {
                    GameObject currentItem = panelHistory.Pop();
                    if (currentItem == panel)
                    {
                        break;
                    }
                    tempStack.Push(currentItem);
                }

                // Devolver los elementos a la pila original
                while (tempStack.Count > 0)
                {
                    panelHistory.Push(tempStack.Pop());
                }
            }

            panelHistory.Push(currentPanel);
        }

        // Activamos el nuevo panel
        panel.SetActive(true);
        currentPanel = panel;
    }
    public void ShowPanelWithoutSaving(GameObject panel)
    {
        // Si hay un panel activo, solo lo desactivamos
        if (currentPanel != null)
        {
            currentPanel.SetActive(false);

            // Remover aparición anterior del panel si existe en el historial
            if (panelHistory.Contains(panel))
            {
                Stack<GameObject> tempStack = new Stack<GameObject>();

                // Sacar elementos hasta encontrar el panel duplicado
                while (panelHistory.Count > 0)
                {
                    GameObject currentItem = panelHistory.Pop();
                    if (currentItem == panel)
                    {
                        break;
                    }
                    tempStack.Push(currentItem);
                }

                // Devolver los elementos a la pila original
                while (tempStack.Count > 0)
                {
                    panelHistory.Push(tempStack.Pop());
                }
            }
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

            // Recuperamos el panel anterior
            GameObject nextPanel = panelHistory.Pop();

            // Si el panel siguiente es igual al actual y hay más paneles en el historial
            if (nextPanel == currentPanel && panelHistory.Count > 0)
            {
                // Guardamos el panel que sacamos por si no hay más en el historial
                GameObject tempPanel = nextPanel;
                // Tomamos el siguiente panel del historial
                nextPanel = panelHistory.Pop();
                // Volvemos a poner el panel temporal en el historial
                panelHistory.Push(tempPanel);
            }

            // Activamos el panel seleccionado
            currentPanel = nextPanel;
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
