using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;

public class SaveData : MonoBehaviour
{
    public static SaveData instance;
    private DatabaseReference dbReference;
    private FirebaseAuth auth;

    private void Awake()
    {
        instance = this;

        // Inicializa las referencias
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
    }

    public async Task<bool> SaveStudentData(JsonData data)
    {
        try
        {
            // Verifica que haya un usuario autenticado
            if (auth.CurrentUser == null)
            {
                Debug.LogError("No hay usuario autenticado. No se pueden guardar los datos.");
                return false;
            }

            // Convierte los datos a JSON
            string json = JsonUtility.ToJson(data);

            // Guarda los datos en la ubicación específica del usuario
            await dbReference.Child("users").Child(auth.CurrentUser.UserId).Child("studentData").SetRawJsonValueAsync(json);

            Debug.Log("Datos guardados correctamente para el usuario: " + auth.CurrentUser.UserId);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al guardar datos: " + e.Message);
            return false;
        }
    }
}