using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager instance;
    
    private DatabaseReference dbReference;
    private FirebaseAuth auth;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Inicializar referencias
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
    }
    
    // Verifica si un usuario tiene datos y, si no, los inicializa
    public async Task EnsureUserDataExists()
    {
        if (auth.CurrentUser == null) return;
        
        string userId = auth.CurrentUser.UserId;
        
        try
        {
            // Verificar si ya existen datos para este usuario
            DataSnapshot snapshot = await dbReference.Child("users").Child(userId).Child("studentData").GetValueAsync();
            
            if (!snapshot.Exists)
            {
                await InitializeUserData(userId);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al verificar datos de usuario: " + e.Message);
        }
    }
    
    // Inicializa los datos para un nuevo usuario
    public async Task InitializeUserData(string userId)
    {
        try
        {
            // Crear estructura inicial de datos
            JsonData initialData = new JsonData();
            initialData.students = new List<StudentData>();
            
            // Convertir a JSON
            string json = JsonUtility.ToJson(initialData);
            
            // Guardar en la base de datos
            await dbReference.Child("users").Child(userId).Child("studentData").SetRawJsonValueAsync(json);
            
            Debug.Log("Datos inicializados para el usuario: " + userId);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al inicializar datos de usuario: " + e.Message);
        }
    }
}
