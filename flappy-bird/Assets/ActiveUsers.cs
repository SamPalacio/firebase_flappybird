using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActiveUsers : MonoBehaviour
{

    public static ActiveUsers Instance;
    private void Awake()
    {
        Instance = this;
    }
    public GameObject playerSlot;
 
    public void AddPlayerSlot(User user)
    {
        string myUserId =PlayerPrefs.GetString("userID");

        if(myUserId!= user.id)
        {
            user.userSlot = Instantiate(playerSlot, playerSlot.transform.parent);
            user.userSlot.gameObject.SetActive(true);
            user.userSlot.GetComponent<Id>().userId = user.id;
            user.userSlot.GetComponentInChildren<TMP_Text>().text = user.userName;
        }


       
    }
    public void RemovePlayerSlot(User user)
    {
        if(user.userSlot != null)
        {
            DestroyImmediate(user.userSlot);

        }

    }

}
