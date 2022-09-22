using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using TMPro;

public class PasswordRecover : MonoBehaviour
{
    public  Button passwordRecoverButton;
    private Coroutine emailCoroutine;
    // Start is called before the first frame update
    void OnEnable()
    {
        passwordRecoverButton.onClick.AddListener(RecoverPassword);

    }
   

    // Update is called once per frame
    public void RecoverPassword()
    {

        if(emailCoroutine== null)
        {
            emailCoroutine = StartCoroutine(SendEmail());

        }


    }

    IEnumerator SendEmail()
    {
        string emailAddress = GameObject.Find("InputEmail").GetComponent<TMP_InputField>().text;
        var auth = FirebaseAuth.DefaultInstance;
        var emailTask = auth.SendPasswordResetEmailAsync(emailAddress);
        yield return new WaitUntil(() => emailTask.IsCompleted);

        if (emailTask.Exception != null)
        {
            ErrorHandler.Instance.SetError($"Check your email");

        }
        else
        {
            if (emailTask.IsCanceled)
            {
                ErrorHandler.Instance.SetError("Check your email");
            }
            else if (emailTask.IsFaulted)
            {
                ErrorHandler.Instance.SetError("Check your email");
            }
            else
            {
                ErrorHandler.Instance.SetError("Password reset email sent successfully.");

            }

        }
        emailCoroutine = null;
    }
}
