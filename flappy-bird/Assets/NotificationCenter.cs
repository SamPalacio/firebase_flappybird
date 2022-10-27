using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static AddFriend;

public class NotificationCenter : MonoBehaviour
{
    public static NotificationCenter instance;

    public GameObject notification;
    public GameObject panel;

    private void Awake()
    {
        instance = this;
    }
    public void AddNotificationFriendRequest(FriendRequest fR)
    {
        panel.SetActive(true);

        Debug.Log(fR.username);
        string text = fR.username +" te ha enviado una solicitud de amigo";
        GameObject newNotification = Instantiate(notification, notification.transform.parent);
        newNotification.SetActive(true);
        newNotification.GetComponent<Notification>().text.text = text;
        newNotification.GetComponent<Notification>().id = fR.requestId;
    }
}
