using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignIn : MonoBehaviour
{
    private Button registerButton;
    private Coroutine _registrationCoroutine;
    DatabaseReference database;
    string m_UserId;

    public static event Action OnUserRegistered;
    public static event Action<Exception> OnUserRegistrationFailed;

    void Awake()
    {
        registerButton = GetComponent<Button>();
        database = FirebaseDatabase.DefaultInstance.RootReference;
        registerButton.onClick.AddListener(HandleRegistrationButtonClicked);

    }

    void HandleRegistrationButtonClicked()
    {
        string email = GameObject.Find("InputEmail").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("InputPassword").GetComponent<TMP_InputField>().text;
        string username = GameObject.Find("InputUsername").GetComponent<TMP_InputField>().text;
        if (!string.IsNullOrEmpty(username))
        {
            _registrationCoroutine = StartCoroutine(RegisterUser(email, password));

        }
        else
        {
            ErrorHandler.Instance.SetError($"Please write a valid username!");

        }
    }

    IEnumerator RegisterUser(string email, string password)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var registerTask= auth.CreateUserWithEmailAndPasswordAsync(email,password);
        yield return new WaitUntil(()=>registerTask.IsCompleted);



        if (registerTask.Exception != null)
        {
            ErrorHandler.Instance.SetError("Check your credentials");
            OnUserRegistrationFailed?.Invoke(registerTask.Exception);
        }
        else
        {
            Debug.Log($"Succesfully registered user {registerTask.Result.Email}");
            string username = GameObject.Find("InputUsername").GetComponent<TMP_InputField>().text;

            UserData data = new UserData();
            data.userName = username;
            string json = JsonUtility.ToJson(data);

            m_UserId = registerTask.Result.UserId;

            var usernameTask = database.Child("users").Child(m_UserId).SetRawJsonValueAsync(json);
            yield return new WaitUntil(() => usernameTask.IsCompleted);
            Debug.Log("username registered");



            OnUserRegistered?.Invoke();

           

        }
    }
}
