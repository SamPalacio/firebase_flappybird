using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    public static string userName;
    public static Action OnLobbyEnter;
    private DatabaseReference database;
    bool dataReady=false;
  

    private void OnEnable()
    {
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthChange;

    }
    private void OnDisable()
    {
        FirebaseAuth.DefaultInstance.StateChanged -= HandleAuthChange;

    }


    void HandleAuthChange(object sender, EventArgs args)
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            SceneManager.LoadScene("Home");
            return;
        }

        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        if (currentUser != null)
        {
          
            SetUserOnline(currentUser.UserId);
            StartCoroutine(WaitData());
        }



    }
   
    public void SetUserOnline(string m_UserId)
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

                userName = snapshot.Value.ToString();
                PlayerPrefs.SetString("username", snapshot.Value.ToString());
                database = FirebaseDatabase.DefaultInstance.RootReference;
                string UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
                PlayerPrefs.SetString("userID", UserId);



                UserOnline user = new UserOnline(UserId, userName);
                string json = JsonUtility.ToJson(user);

                database.Child("online-users").Child(UserId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
                {

                    dataReady = true;
                });

               
               

              

            }
        });


      
    }

    IEnumerator WaitData()
    {
        yield return new WaitUntil(() => dataReady == true);
        
            OnLobbyEnter?.Invoke();
        
    }
  


    void NextScene()
    {
        SceneManager.LoadScene("main");

    }

    public class UserOnline {

        public string id;
        public string username;

        public UserOnline(string id, string username)
        {
            this.id = id;
            this.username = username;
        }
    }
}
