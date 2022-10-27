using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public UnityEvent OnGameOver=new UnityEvent();
    public static  GameManager instance;
    private DatabaseReference mDatabase;

    private void Awake()
    {
        instance = this;
    }


    private void OnEnable()
    {
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthStateChange;

    }
    private void OnDisable()
    {
        FirebaseAuth.DefaultInstance.StateChanged -= HandleAuthStateChange;

    }


    public void HandleAuthStateChange(object sender, EventArgs e)
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            SceneManager.LoadScene("Home");

        }
    }

    void Start()
    {
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
        OnGameOver?.Invoke();
        Time.timeScale = 0;
    }
    public void Replay()
    {
        SceneManager.LoadScene("main");
    }

    private void OnApplicationQuit()
    {
        
            mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
            string UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            mDatabase.Child("online-users").Child(UserId).SetValueAsync(null);

        
    }
}




    

 

