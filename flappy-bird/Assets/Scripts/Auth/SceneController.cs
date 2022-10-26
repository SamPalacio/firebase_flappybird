using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using System;

public class SceneController : MonoBehaviour
{
    private void OnEnable()
    {
        LogIn.OnLoginSuceeded += NextScene;

    }
    private void OnDisable()
    {
        LogIn.OnLoginSuceeded -= NextScene;
    }

    void NextScene(FirebaseUser user)
    {
        SceneManager.LoadScene("Lobby");
    }
}
