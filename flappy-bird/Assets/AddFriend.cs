using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;
using Google.MiniJSON;
using System;

public class AddFriend : MonoBehaviour
{
    public TMP_Text username;
    public GameObject requestPanel;
    private User selectedUser;
    private DatabaseReference mDatabase;



    private void OnEnable()
    {
        Lobby.OnLobbyEnter += InitRequestController;

    }
    private void OnDisable()
    {
        Lobby.OnLobbyEnter -= InitRequestController;

    }

    public void InitRequestController()
    {


        string myId = PlayerPrefs.GetString("userID");

        var userOnlineRef = FirebaseDatabase.DefaultInstance
        .GetReference($"users/{myId}");

        userOnlineRef.ChildChanged += HandleRequests;

    }

    private void HandleRequests(object sender, ChildChangedEventArgs args)
    {

        if (args.DatabaseError != null)
        {
            Debug.Log(args.DatabaseError.Message);
            return;
        }
        Debug.Log(args.Snapshot.Value);
    }

    public void ShowRequestInfo(Id id)
    {
        selectedUser = UsersOnlineController.instance.activeUsers[id.userId];
        requestPanel.SetActive(true);
        username.text = selectedUser.userName;
    }

    public void SendFriendRequest()
    {

        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;

        string myId = PlayerPrefs.GetString("userID");
        string requestId = myId + "r";

        FriendRequest friendRequest = new FriendRequest(myId, false, requestId);
        Debug.Log("Send"+ myId);

        string json = JsonUtility.ToJson(friendRequest);
        mDatabase.Child("users").Child(selectedUser.id).Child("request").Child(requestId).SetRawJsonValueAsync(json);



     }


   
    [System.Serializable]
    public class FriendRequest
    {
        public string sender;
        public bool accepted;
        public string requestId;
        public FriendRequest(string sender, bool accepted, string requestId)
        {
            this.sender = sender;
            this.accepted = accepted;
            this.requestId = requestId;
        }
    }
}
