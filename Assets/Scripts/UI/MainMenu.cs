using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [SerializeField] private InputField usernameInput;
    [SerializeField] private InputField passwordInput;
    
    [SerializeField] private InputField registerUsernameInput;
    [SerializeField] private InputField registerPasswordInput;
    [SerializeField] private InputField nameInput;
    [SerializeField] private InputField emailInput;

    [SerializeField] private TextMeshProUGUI logInButtonText;
    [SerializeField] private Button onlineButton;
    [SerializeField] private Button registerButton;
    [SerializeField] private MenuControl menuControl;
    [SerializeField] private TextMeshProUGUI loginResultLabel;
    [SerializeField] private TextMeshProUGUI registerResultLabel;

    public void DoLogin() {
        if (VariableHolder.User.IsLoggedIn()) {
            VariableHolder.User.Username = VariableHolder.User.Password = null;
            logInButtonText.text = "Log In";
            onlineButton.interactable = false;
            registerButton.interactable = true;
        } else {
            menuControl.OnStateChanged(MenuStates.MainMenuLogin);
        }
    }

    public void LogIn() {
        string username = usernameInput.text;
        string password = passwordInput.text;

        StartCoroutine(LoginAsync(username, password));
    }

    public void Register() {
        string username = registerUsernameInput.text;
        string password = registerPasswordInput.text;
        string name = nameInput.text;
        string email = emailInput.text;

        StartCoroutine(RegisterAsync(new UserRegisterRequest(name, username, password, email)));
    }

    public void ExitGame() {
        Application.Quit();
    }

    private IEnumerator LoginAsync(string username, string password) {
        UnityWebRequest loginRequest = APIHelper.AttemptLogIn(username, password);
        yield return loginRequest.SendWebRequest();

        if (loginRequest.responseCode == 200) {
            VariableHolder.User.UserID = int.Parse(loginRequest.downloadHandler.text);
            VariableHolder.User.Username = username;
            VariableHolder.User.Password = password;

            logInButtonText.text = "Log Out";
            onlineButton.interactable = true;
            registerButton.interactable = false;
            loginResultLabel.text = "Login successful.";
        } else {
            loginResultLabel.text = "Invalid username or password";
        }

        menuControl.GoToPreviousState();
        menuControl.OnStateChanged(MenuStates.MainMenuLoginResult, false);
    }

    private IEnumerator RegisterAsync(UserRegisterRequest userRegister) {
        UnityWebRequest registerRequest = APIHelper.Register(userRegister);
        yield return registerRequest.SendWebRequest();

        if (registerRequest.responseCode == 200) {
            //VariableHolder.User.Username = userRegister.username;
            //VariableHolder.User.Password = userRegister.password;

            //logInButtonText.text = "Log Out";
            //onlineButton.interactable = true;
            //registerButton.interactable = false;
            registerResultLabel.text = "Registered successfully.";
        } else {
            registerResultLabel.text = "Username or email already taken."; //TODO: maybe make a specific error message
        }

        menuControl.GoToPreviousState();
        menuControl.OnStateChanged(MenuStates.MainMenuRegisterResult, false);
    }
}
