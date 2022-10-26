using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] TMP_Text userName;

    private void OnEnable()
    {
        Lobby.OnLobbyEnter += SetUsernameLabel;   
    }
    private void OnDisable()
    {
        Lobby.OnLobbyEnter -= SetUsernameLabel;
    }

    public void SetUsernameLabel()
    {
        userName.text = Lobby.userName;
    }

}
