using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;

public class AddFriend : MonoBehaviour
{
    public TMP_Text username;
    public GameObject requestPanel;
    private User selectedUser;
   public void ShowRequestInfo(Id id)
    {
        selectedUser = UsersOnlineController.instance.activeUsers[id.userId];
        requestPanel.SetActive(true);
        username.text = selectedUser.userName;
    }

    public void SendFriendRequest()
    {


       

     }
}
