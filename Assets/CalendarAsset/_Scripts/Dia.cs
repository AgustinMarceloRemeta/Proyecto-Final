using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dia : MonoBehaviour
{
    private TextMeshProUGUI _diaTexto;

    Button button;
    bool selected= false;
    bool attended= false;
    public bool weekend;
    private void Awake()
    {
        _diaTexto = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponentInChildren<Button>();
    }

    private void Start()
    {
    }

    public void SetButton()
    {
        if (weekend) _diaTexto.color = Color.grey;
        else button.onClick.AddListener(() => OnClickButton());
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
        if (!selected)
        {
            _diaTexto.color = Color.green;
            selected = true;
            attended = true;
        }
        else
        {
            if (attended)
            {
                _diaTexto.color = Color.red;
                attended = false;
            }
            else
            {
                _diaTexto.color= Color.green;
                attended = true;
            }
        }
    }
}
