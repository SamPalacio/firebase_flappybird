using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;
using Google.MiniJSON;

public class AddFriend : MonoBehaviour
{
    public TMP_Text username;
    public GameObject message;
    public GameObject requestPanel;
    private User selectedUser;
    private DatabaseReference mDatabase;
    Dictionary<string, FriendRequest> mFriends = new Dictionary<string, FriendRequest>();


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

        userOnlineRef.ValueChanged += HandleRequests;

    }

    private void HandleRequests(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            return;
        }

        if (args.Snapshot.HasChildren)
        {
            Dictionary<string, object> userList = (Dictionary<string, object>)args.Snapshot.Value;
            if (userList.ContainsKey("friendRequests"))
            {
                Dictionary<string, object> request = (Dictionary<string, object>)userList["friendRequests"];
                foreach (var userDoc in request)
                {
                    Dictionary<string, object> userOnline = (Dictionary<string, object>)userDoc.Value;
                    FriendRequest fR = new FriendRequest((string)userOnline["sender"], (bool)userOnline["accepted"], (string)userOnline["requestId"], (string)userOnline["username"]);
                    if (!mFriends.ContainsKey(fR.requestId))
                    {
                        mFriends.Add(fR.requestId, fR);
                        NotificationCenter.instance.AddNotificationFriendRequest(fR);

                    }
                }
            }


            
        }


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
        string username = PlayerPrefs.GetString("username");
        string requestId = myId + "r";

        FriendRequest friendRequest = new FriendRequest(myId, false, requestId,username);
        Debug.Log("Send"+ myId);

        string json = JsonUtility.ToJson(friendRequest);
        mDatabase.Child("users").Child(selectedUser.id).Child("friendRequests").Child(requestId).SetRawJsonValueAsync(json);

        requestPanel.gameObject.SetActive(false);
        message.gameObject.SetActive(true);
     }


    [System.Serializable]
    public class FriendRequest
    {
        public string sender;
        public bool accepted;
        public string requestId;
        public string username;
        public FriendRequest(string sender, bool accepted, string requestId, string username)
        {
            this.sender = sender;
            this.accepted = accepted;
            this.requestId = requestId;
            this.username = username;
        }
    }
}
