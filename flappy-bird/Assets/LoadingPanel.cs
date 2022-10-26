using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    private void OnEnable()
    {
        Lobby.OnLobbyEnter += HideScreen;

    }
    private void OnDisable()
    {
        Lobby.OnLobbyEnter -= HideScreen;

    }

    private void HideScreen()
    {
      this.gameObject.SetActive(false);
    }
}
