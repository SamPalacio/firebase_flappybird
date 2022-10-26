using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Firebase.Database;

public class ButtonLogOut : MonoBehaviour,IPointerClickHandler
{
    private DatabaseReference database;

   

    public void OnPointerClick(PointerEventData eventData)
    {

        UsersOnlineController.instance.SetUserOffline();
        FirebaseAuth.DefaultInstance.SignOut();
        SceneManager.LoadScene(0);

      

    }

    

  


}
