using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;

public class MatchMaking : MonoBehaviour
{
    DatabaseReference mDatabase;
   public TMP_Text playerList;
   public TMP_Text playerCount;
    public int minPlayers;

   public void SearchMatch()
    {
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;

        FirebaseDatabase.DefaultInstance
        .GetReference("partidas")
        .GetValueAsync().ContinueWithOnMainThread(task =>
        {
          
            if(!task.Result.Exists)
            {
                CreateMatch();
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                Dictionary<string, object> partidas = (Dictionary<string, object>)snapshot.Value;
                foreach (var item in partidas)
                {
                    Dictionary<string, object> partida = (Dictionary<string, object>)item.Value;

                    bool isFull = (bool)partida["full"];
                    if (!isFull)
                    {
                        string userId = PlayerPrefs.GetString("userID");
                        string username = PlayerPrefs.GetString("username");

                        Dictionary<string, object> players = (Dictionary<string, object>)partida["players"];
                        int playerIndex = players.Keys.Count + 1;
                        mDatabase.Child("partidas").Child(item.Key).Child("players").Child("player" + playerIndex).SetValueAsync(username);
                    }
                }


            }
           
            

        });


         

    }
    public void CreateMatch()
    {
        string userId = PlayerPrefs.GetString("userID");
        string username = PlayerPrefs.GetString("username");
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        mDatabase.Child("partidas").Child(userId).Child("players").Child("player1").SetValueAsync(username);
        mDatabase.Child("partidas").Child(userId).Child("full").SetValueAsync(false);
    }






}
