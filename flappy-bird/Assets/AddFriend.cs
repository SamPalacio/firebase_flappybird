using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddFriend : MonoBehaviour
{
    public TMP_Text username;
    public GameObject requestPanel;
    private User selectedUser;
   public void ShowRequestInfo(Id id)
    {
        selectedUser = UsersOnlineController.activeUsers[id.userId];
        requestPanel.SetActive(true);
        username.text = selectedUser.userName;
    }
}
