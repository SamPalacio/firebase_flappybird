using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static AddFriend;

public class NotificationCenter : MonoBehaviour
{
    public static NotificationCenter instance;

    public GameObject notification;
    public GameObject notificationNewFriend;
    public GameObject popupNotificacion;
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
        newNotification.GetComponent<Notification>().fR = fR;
    }
    public void AddNotificationFriendAccepted(OwnFriend friend)
    {
        panel.SetActive(true);

        Debug.Log(friend.username);
        string text = friend.username + " ha comenzado a ser tu amigo";
        GameObject newNotification = Instantiate(notificationNewFriend, notification.transform.parent);
        newNotification.SetActive(true);
        newNotification.GetComponent<Notification>().text.text = text;
    }
    public void AddPopUpNotification(string message)
    {
        StartCoroutine(ShowNotification(message));
    }

    IEnumerator ShowNotification(string message)
    {
        popupNotificacion.SetActive(true);
        popupNotificacion.GetComponentInChildren<TMP_Text>().text = message;
        yield return new WaitForSeconds(5);
        popupNotificacion.SetActive(false);

    }

}
