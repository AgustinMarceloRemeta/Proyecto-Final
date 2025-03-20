using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;
using TMPro;

public class LoadData : MonoBehaviour
{
    public JsonData data;
    public static LoadData instance;

    private FirebaseAuth auth;
    private DatabaseReference dbReference;
    private FirebaseUser currentUser;

    private void Awake()
    {
        instance = this;
        InitializeFirebase();
    }

    private async void InitializeFirebase()
    {
        // Inicializa Firebase
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            auth = FirebaseAuth.DefaultInstance;
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;

            // Verifica si hay un usuario autenticado
            currentUser = auth.CurrentUser;

            if (currentUser != null)
            {
                Debug.Log("Usuario autenticado: " + currentUser.UserId);

                // Carga los datos específicos del usuario actual
                data = await ReturnData();

                // Una vez que los datos están cargados, configura el resto
                if (data.students.Count > 0)
                {
                    DataManager.instance.OrderStudents(data.students);
                    DataManager.instance.CheckAttendances(data.students);


                }
                DataManager.instance.SetCourses();
            }
            else
            {
                Debug.LogError("No hay usuario autenticado. No se pueden cargar los datos.");
            }
        }
        else
        {
            Debug.LogError("No se pudo resolver las dependencias de Firebase: " + dependencyStatus.ToString());
        }
    }

    public async Task<JsonData> ReturnData()
    {
        JsonData data = new JsonData();
        data.students = new List<StudentData>();

        try
        {
            if (currentUser != null)
            {
                // Obtiene los datos desde Firebase para el usuario actual
                DataSnapshot snapshot = await dbReference.Child("users").Child(currentUser.UserId).Child("studentData").GetValueAsync();

                if (snapshot.Exists && snapshot.ChildrenCount > 0)
                {
                    // Procesa los datos JSON desde Firebase
                    string json = snapshot.GetRawJsonValue();
                    data = JsonUtility.FromJson<JsonData>(json);
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

        return data;
    }

    public async Task<bool> ReloadData()
    {
        try
        {
            // Asegurarse de que el usuario sigue autenticado
            if (auth.CurrentUser == null)
            {
                Debug.LogError("No hay usuario autenticado para recargar datos.");
                return false;
            }

            currentUser = auth.CurrentUser;
            data = await ReturnData();

            if (data.students.Count > 0)
                DataManager.instance.OrderStudents(data.students);

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al recargar los datos: " + e.Message);
            return false;
        }
    }


    public void Save()
    {
        SaveData.instance.SaveStudentData(data);
    }
}