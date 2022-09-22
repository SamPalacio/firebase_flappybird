using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogIn : MonoBehaviour
{
    private Button  loginButton;
    private Coroutine loginCoroutine;

    public static event Action<FirebaseUser> OnLoginSuceeded;
    public static event Action<Exception> OnLoginFailed;

    void Awake()
    {
        loginButton = GetComponent<Button>();
        loginButton.onClick.AddListener(UserLogIn);
    }
    private void OnEnable()
    {
        SignIn.OnUserRegistered += UserLogIn;
    }
    private void OnDisable()
    {
        SignIn.OnUserRegistered -= UserLogIn;

    }
    public void UserLogIn()
    {
        string email = GameObject.Find("InputEmail").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("InputPassword").GetComponent<TMP_InputField>().text;

        if (loginCoroutine == null)
        {
            loginCoroutine= StartCoroutine(LoginUser(email, password));
        }
    }

    IEnumerator LoginUser(string email, string password)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            ErrorHandler.Instance.SetError("Check your credentials");

            OnLoginFailed?.Invoke(loginTask.Exception);
        }
        else
        {
            Debug.Log($"Succesfully login user {loginTask.Result.Email}");
            OnLoginSuceeded?.Invoke(loginTask.Result);
        }
        loginCoroutine = null;
    }

}
