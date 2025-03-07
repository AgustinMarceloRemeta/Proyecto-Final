using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Unity.VisualScripting;
using System.Collections.Generic;

public class FirebaseAuthManager : MonoBehaviour
{
    // Singleton
    public static FirebaseAuthManager instance;

    // Firebase
    private FirebaseAuth auth;
    private FirebaseUser currentUser;
    private DatabaseReference dbReference;

    // Estado de la sesión
    public bool IsLoggedIn { get { return currentUser != null; } }

    // Referencias a la UI (asignar en el Inspector)
    [Header("Pantallas")]
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject loadingPanel;
    public GameObject mainAppPanel;

    [Header("Login UI")]
    public TMP_InputField loginEmailField;
    public TMP_InputField loginPasswordField;
    public Button loginButton;
    public TextMeshProUGUI loginErrorText;
    public Button goToRegisterButton;

    [Header("Register UI")]
    public TMP_InputField registerEmailField;
    public TMP_InputField registerPasswordField;
    public TMP_InputField confirmPasswordField;
    public Button registerButton;
    public TextMeshProUGUI registerErrorText;
    public Button backToLoginButton;

    private void Awake()
    {
        // Configuración del singleton
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

        // Inicializar Firebase
        InitializeFirebase();
    }

    private async void InitializeFirebase()
    {
        // Mostrar pantalla de carga
        ShowLoadingPanel(true);

        // Verificar dependencias de Firebase
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            // Inicializar Auth y Database
            auth = FirebaseAuth.DefaultInstance;
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;

            // Configurar el detector de cambios de estado de autenticación
            auth.StateChanged += AuthStateChanged;

            // Verificar si hay una sesión activa
            AuthStateChanged(this, null);
        }
        else
        {
            Debug.LogError($"No se pudieron resolver las dependencias de Firebase: {dependencyStatus}");
            ShowLoginPanel();
            SetErrorMessage(loginErrorText, "Error al conectar con el servidor. Intenta más tarde.");
        }

        // Configurar botones
        SetupUIButtons();
    }

    private void SetupUIButtons()
    {
        // Botones de login
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        goToRegisterButton.onClick.AddListener(ShowRegisterPanel);

        // Botones de registro
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        backToLoginButton.onClick.AddListener(ShowLoginPanel);
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        // Actualizar referencia al usuario actual
        if (auth.CurrentUser != currentUser)
        {
            bool wasSignedIn = currentUser != null;
            currentUser = auth.CurrentUser;
            bool isSignedIn = currentUser != null;

            if (wasSignedIn != isSignedIn)
            {
                if (isSignedIn)
                {
                    // Usuario inició sesión
                    Debug.Log($"Usuario autenticado: {currentUser.Email}");
                    ShowMainAppPanel();
                }
                else
                {
                    // Usuario cerró sesión
                    Debug.Log("Usuario desconectado");
                    ShowLoginPanel();
                }
            }
        }

        // Ocultar pantalla de carga si estaba visible
        ShowLoadingPanel(false);
    }

    #region UI Management

    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        loadingPanel.SetActive(false);
        mainAppPanel.SetActive(false);
        ClearErrorMessages();
        ClearInputFields();
    }

    public void ShowRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        loadingPanel.SetActive(false);
        mainAppPanel.SetActive(false);
        ClearErrorMessages();
        ClearInputFields();
    }

    public void ShowMainAppPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        loadingPanel.SetActive(false);
        mainAppPanel.SetActive(true);
    }

    public void ShowLoadingPanel(bool show)
    {
        loadingPanel.SetActive(show);
    }

    private void ClearErrorMessages()
    {
        loginErrorText.text = "";
        registerErrorText.text = "";
    }

    private void ClearInputFields()
    {
        loginEmailField.text = "";
        loginPasswordField.text = "";
        registerEmailField.text = "";
        registerPasswordField.text = "";
        confirmPasswordField.text = "";
    }

    private void SetErrorMessage(TextMeshProUGUI errorText, string message)
    {
        errorText.text = message;
    }

    #endregion

    #region Authentication Methods

    public async void OnLoginButtonClicked()
    {
        string email = loginEmailField.text;
        string password = loginPasswordField.text;

        // Validaciones básicas
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            SetErrorMessage(loginErrorText, "Por favor, completa todos los campos");
            return;
        }

        // Deshabilitar botones e iniciar carga
        loginButton.interactable = false;
        ShowLoadingPanel(true);

        try
        {
            // Intento de inicio de sesión
            await auth.SignInWithEmailAndPasswordAsync(email, password);
            // No necesitamos hacer nada aquí porque el evento AuthStateChanged se activará
        }
        catch (FirebaseException e)
        {
            // Manejar errores específicos
            string errorMessage = InterpretFirebaseError(e);
            SetErrorMessage(loginErrorText, errorMessage);
            ShowLoadingPanel(false);
        }
        finally
        {
            loginButton.interactable = true;
        }
    }

    public async void OnRegisterButtonClicked()
    {
        string email = registerEmailField.text;
        string password = registerPasswordField.text;
        string confirmPassword = confirmPasswordField.text;

        // Validaciones básicas
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            SetErrorMessage(registerErrorText, "Por favor, completa todos los campos");
            return;
        }

        if (password != confirmPassword)
        {
            SetErrorMessage(registerErrorText, "Las contraseñas no coinciden");
            return;
        }

        if (password.Length < 6)
        {
            SetErrorMessage(registerErrorText, "La contraseña debe tener al menos 6 caracteres");
            return;
        }

        // Deshabilitar botones e iniciar carga
        registerButton.interactable = false;
        ShowLoadingPanel(true);

        try
        {
            // Intento de registro
            var authResult = await auth.CreateUserWithEmailAndPasswordAsync(email, password);

            // Si el registro es exitoso, inicializa los datos del usuario
            if (authResult.User != null)
            {
                await InitializeUserData(authResult.User.UserId);
            }

            // El inicio de sesión se hará automáticamente y el evento AuthStateChanged se activará
        }
        catch (FirebaseException e)
        {
            // Manejar errores específicos
            string errorMessage = InterpretFirebaseError(e);
            SetErrorMessage(registerErrorText, errorMessage);
            ShowLoadingPanel(false);
        }
        finally
        {
            registerButton.interactable = true;
        }
    }

    // Método para inicializar la estructura de datos del usuario
    private async Task InitializeUserData(string userId)
    {
        try
        {
            // Crear estructura inicial de datos
            JsonData initialData = new JsonData();
            initialData.students = new List<StudentData>();

            // Convertir a JSON
            string json = JsonUtility.ToJson(initialData);

            // Guardar en la base de datos bajo el ID del usuario
            await dbReference.Child("users").Child(userId).Child("studentData").SetRawJsonValueAsync(json);

            Debug.Log("Datos inicializados para el nuevo usuario: " + userId);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al inicializar datos del usuario: " + e.Message);
        }
    }

    public async void LogOut()
    {
        if (auth != null)
        {
            ShowLoadingPanel(true);
            await Task.Delay(500); // Pequeño retraso para mostrar el panel de carga
            auth.SignOut();
        }
    }

    private string InterpretFirebaseError(FirebaseException e)
    {
        int errorCode = e.ErrorCode;
        switch (errorCode)
        {
            case 17020: // "ERROR_USER_NOT_FOUND"
            case 17009: // "ERROR_WRONG_PASSWORD"
                return "Email o contraseña incorrectos";
            case 17008: // "ERROR_INVALID_EMAIL"
                return "Formato de email inválido";
            case 17007: // "ERROR_EMAIL_ALREADY_IN_USE"
                return "El email ya está registrado";
            case 17026: // "ERROR_WEAK_PASSWORD"
                return "La contraseña es demasiado débil";
            case 17011: // "ERROR_REQUIRES_RECENT_LOGIN"
                return "Esta operación es sensible y requiere autenticación reciente";
            case 17010: // "ERROR_CREDENTIAL_ALREADY_IN_USE"
                return "Esta credencial ya está asociada a una cuenta diferente";
            case 17023: // "ERROR_OPERATION_NOT_ALLOWED"
                return "Operación no permitida";
            default:
                return $"Error de autenticación: {e.Message}";
        }
    }

    #endregion
}