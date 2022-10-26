using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Firebase.Database;
using System.Xml;
using System;
using Firebase;

public class UsersOnlineController : MonoBehaviour
{
    public  Dictionary<string, User> activeUsers = new Dictionary<string, User>();

    public static UsersOnlineController instance;
    DatabaseReference mDatabase;
    


    private void Awake()
    {
        instance=this;
        activeUsers = new Dictionary<string, User>();
    }
    void Start()
    {
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
    }
    private void OnEnable()
    {
        Lobby.OnLobbyEnter += InitUsersOnlineController;

    }
    private void OnDisable()
    {
        Lobby.OnLobbyEnter -= InitUsersOnlineController;

    }

    public void InitUsersOnlineController()
    {

        var userOnlineRef = FirebaseDatabase.DefaultInstance
        .GetReference("online-users");

        userOnlineRef.ChildAdded += HandleChildAdded;
        userOnlineRef.ChildRemoved += HandleChildRemoved;

    }

   

    private void HandleChildAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.Log("error");
           return;
        }


        
        Dictionary<string, object> userConnected = (Dictionary<string, object>)args.Snapshot.Value;
        
            User user = new User((string)userConnected["username"], (string)userConnected["id"]);

            ActiveUsers.Instance.AddPlayerSlot(user);

            activeUsers.Add((string)userConnected["id"], user);

            Debug.Log(userConnected["username"] + " is online");
        

           
        
       
    }
    private void HandleChildRemoved(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;

        }


        Dictionary<string, object> userDisconnected = (Dictionary<string, object>)args.Snapshot.Value;

        
            User user = activeUsers[(string)userDisconnected["id"]];


            ActiveUsers.Instance.RemovePlayerSlot(user);

           
                activeUsers.Remove((string)userDisconnected["id"]);

     


        Debug.Log(userDisconnected["username"] + " is offline");





    }




    public void SetUserOffline()
    {
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        string UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        mDatabase.Child("online-users").Child(UserId).SetValueAsync(null);

    }

    void OnApplicationQuit()
    {
        SetUserOffline();
    }


   
}
