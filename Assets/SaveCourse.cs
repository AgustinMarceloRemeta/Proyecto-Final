using Firebase.Auth;
using Firebase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SaveCourse : MonoBehaviour
{
    public static SaveCourse instance;
    private DatabaseReference dbReference;
    private FirebaseAuth auth;

    private void Awake()
    {
        instance = this;

        // Inicializa las referencias
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
    }

    public async Task<bool> SaveCourseData(List<DataCourse> listCouse)
    {
        CourseList data = new CourseList();
        data.courses = listCouse;
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
            await dbReference.Child("users").Child(auth.CurrentUser.UserId).Child("courses").SetRawJsonValueAsync(json);

            Debug.Log("Datos guardados correctamente para el usuario: " + auth.CurrentUser.UserId);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al guardar datos: " + e.Message);
            return false;
        }
    }

    public async Task<List<DataCourse>> ReturnData()
    {
        CourseList data = new CourseList();

        try
        {
            if (auth.CurrentUser != null)
            {
                // Obtiene los datos desde Firebase para el usuario actual
                DataSnapshot snapshot = await dbReference.Child("users").Child(auth.CurrentUser.UserId).Child("courses").GetValueAsync();

                if (snapshot.Exists && snapshot.ChildrenCount > 0)
                {
                    // Procesa los datos JSON desde Firebase
                    string json = snapshot.GetRawJsonValue();
                    data = JsonUtility.FromJson<CourseList>(json);
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

        return data.courses;
    }
}
