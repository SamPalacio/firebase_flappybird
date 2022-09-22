using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class InfoUiController : MonoBehaviour
{
    [SerializeField] TMP_Text userLabel;
    [SerializeField] TMP_Text userScore;

    private void OnEnable()
    {
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthChange;
    }
    private void OnDisable()
    {
        FirebaseAuth.DefaultInstance.StateChanged -= HandleAuthChange;
    }
    // Update is called once per frame
    void HandleAuthChange(object sender, EventArgs args)
    {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        if (currentUser != null)
        {
            SetUserLabel(currentUser.UserId);
            SetUserScore(currentUser.UserId);
        }
        


    }
    void SetUserScore(string m_UserId)
    {



        FirebaseDatabase.DefaultInstance
         .GetReference("users/" + m_UserId + "/score")
         .GetValueAsync().ContinueWithOnMainThread(task => {
             if (task.IsFaulted)
             {
                 ErrorHandler.Instance.SetError(task.Exception.Message);
             }
             else if (task.IsCompleted)
             {
                 DataSnapshot snapshot = task.Result;
                 Debug.Log(snapshot.Value.ToString());

                 userScore.text = "Score: " + snapshot.Value.ToString();

                 PlayerPrefs.SetInt("score", int.Parse(snapshot.Value.ToString()));

             }
         });

    }

    void SetUserLabel(string m_UserId)
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("users/" + m_UserId + "/userName")
        .GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                ErrorHandler.Instance.SetError(task.Exception.Message);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
              
                userLabel.text = "Actual user name: "+snapshot.Value.ToString();
                PlayerPrefs.SetString("username", snapshot.Value.ToString());

            }
        });
    }
}
