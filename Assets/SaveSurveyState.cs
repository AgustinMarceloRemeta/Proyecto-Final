using Firebase.Auth;
using Firebase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SaveSurveyState : MonoBehaviour
{
    private DatabaseReference dbReference;
    private FirebaseAuth auth;

    private void Awake()
    {
        // Inicializa las referencias
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
    }

    public async Task<bool> SaveCompleteSurvey(bool competed)
    {
        try
        {
            if (auth.CurrentUser == null)
            {
                Debug.LogError("No hay usuario autenticado. No se pueden guardar los datos.");
                return false;
            }



            // Guarda los datos en la ubicación específica del usuario
            await dbReference.Child("users").Child(auth.CurrentUser.UserId).Child("SurveyComplete").SetValueAsync(competed);

            Debug.Log("Datos guardados correctamente para el usuario: " + auth.CurrentUser.UserId);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al guardar datos: " + e.Message);
            return false;
        }
    }

    public async Task<bool> ReturnData()
    {
        bool state = false;
        try
        {
            if (auth.CurrentUser != null)
            {
                // Obtiene los datos desde Firebase para el usuario actual
                DataSnapshot snapshot = await dbReference.Child("users").Child(auth.CurrentUser.UserId).Child("SurveyComplete").GetValueAsync();
                if (snapshot.Exists)
                {
                    // Lee el valor booleano directamente
                    state = (bool)snapshot.Value;
                }
                Debug.Log("Datos del usuario cargados correctamente desde Firebase");
            }
            else
            {
                Debug.LogError("Usuario no autenticado. No se pueden cargar los datos.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al cargar datos desde Firebase: " + e.Message);
        }
        return state;
    }

    public void Save(bool value)
    {
        SaveCompleteSurvey(value);
    }
}
