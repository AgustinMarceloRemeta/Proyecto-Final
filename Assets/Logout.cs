using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
    private FirebaseAuth auth;

    void Start()
    {
        // Obtener la instancia de autenticación
        auth = FirebaseAuth.DefaultInstance;
    }

    public void CerrarSesion()
    {
        if (auth != null && auth.CurrentUser != null)
        {
            // Cerrar sesión
            auth.SignOut();
            Debug.Log("Se ha cerrado la sesión correctamente");

            // Eliminar preferencias de usuario guardadas
            PlayerPrefs.DeleteKey("user_email");
            PlayerPrefs.DeleteKey("user_password");
            PlayerPrefs.DeleteKey("isLoggedIn");
            PlayerPrefs.Save();

            // Opcional: Redirigir a la escena de login
            // SceneManager.LoadScene("LoginScene");
        }
        else
        {
            Debug.Log("No hay ningún usuario con sesión activa");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerPrefs.DeleteKey("user_email");
            PlayerPrefs.DeleteKey("user_password");
            PlayerPrefs.DeleteKey("isLoggedIn");
            PlayerPrefs.Save();
            CerrarSesion();
        }
    }
}