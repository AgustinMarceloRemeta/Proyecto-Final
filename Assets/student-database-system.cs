using UnityEngine;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;
using System;


public class DatabaseManager : MonoBehaviour
{
    private const string ADMIN_EMAIL = "admin@example.com"; // Cambiar por tu email de administrador
    private const string ADMIN_PASSWORD = "password123"; // Cambiar por tu contraseña

    private FirebaseAuth auth;
    private DatabaseReference dbReference;
    private FirebaseUser currentUser;

    private void Awake()
    {
        InitializeFirebase();
    }

    private async void InitializeFirebase()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            auth = FirebaseAuth.DefaultInstance;
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        }
        else
        {
            Debug.LogError("No se pudo resolver las dependencias de Firebase: " + dependencyStatus.ToString());
        }
    }

    public async Task<bool> ValidateCredentials(string email, string password)
    {
        try
        {
            var result = await auth.SignInWithEmailAndPasswordAsync(email, password);
            currentUser = result.User;
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Error en autenticación: " + e.Message);
            return false;
        }
    }

    public async Task<List<StudentData>> GetStudentList()
    {
        if (currentUser == null) return null;

        try
        {
            var snapshot = await dbReference.Child("students").GetValueAsync();
            List<StudentData> students = new List<StudentData>();

            foreach (var childSnapshot in snapshot.Children)
            {
                var jsonData = childSnapshot.GetRawJsonValue();
                StudentData student = JsonUtility.FromJson<StudentData>(jsonData);
                students.Add(student);
            }

            return students;
        }
        catch (Exception e)
        {
            Debug.LogError("Error obteniendo lista de estudiantes: " + e.Message);
            return null;
        }
    }

    public async Task<bool> AddStudent(StudentData newStudent)
    {
        if (currentUser == null) return false;

        try
        {
            string key = dbReference.Child("students").Push().Key;
            string json = JsonUtility.ToJson(newStudent);
            await dbReference.Child("students").Child(key).SetRawJsonValueAsync(json);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Error agregando estudiante: " + e.Message);
            return false;
        }
    }
}

public class LoginUI : MonoBehaviour
{
    public TMPro.TMP_InputField emailInput;
    public TMPro.TMP_InputField passwordInput;
    public GameObject loginPanel;
    public GameObject studentListPanel;
    public TMPro.TMP_Text errorText;
    public Transform studentListContent;
    public GameObject studentPrefab;
    public GameObject loadingPanel;

    private DatabaseManager databaseManager;

    void Start()
    {
        databaseManager = FindObjectOfType<DatabaseManager>();
        loginPanel.SetActive(true);
        studentListPanel.SetActive(false);
        loadingPanel.SetActive(false);
        errorText.gameObject.SetActive(false);
    }

    public async void OnLoginButton()
    {
        loadingPanel.SetActive(true);
        string email = emailInput.text;
        string password = passwordInput.text;

        bool isAuthenticated = await databaseManager.ValidateCredentials(email, password);

        if (isAuthenticated)
        {
            List<StudentData> students = await databaseManager.GetStudentList();
            if (students != null)
            {
                loginPanel.SetActive(false);
                studentListPanel.SetActive(true);
                DisplayStudents(students);
            }
        }
        else
        {
            errorText.gameObject.SetActive(true);
            errorText.text = "Usuario o contraseña incorrectos";
        }
        
        loadingPanel.SetActive(false);
    }

    private void DisplayStudents(List<StudentData> students)
    {
        foreach (Transform child in studentListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (StudentData student in students)
        {
            GameObject studentObj = Instantiate(studentPrefab, studentListContent);
            TMPro.TMP_Text studentText = studentObj.GetComponent<TMPro.TMP_Text>();
            studentText.text = $"Nombre: {student.name}";
        }
    }
}
