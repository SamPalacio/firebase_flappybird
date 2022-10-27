using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;
using System;
using UnityEngine.SceneManagement;

public class MatchMaking : MonoBehaviour
{
    DatabaseReference mDatabase;
   public TMP_Text playerList;
   public TMP_Text playerCount;
   public TMP_Text fullMatch;
    public int minPlayers;


   public void SearchMatch()
    {
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        bool matchFound = false;
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
                        matchFound = true;
                        HandleMatch(item.Key);

                        if (playerIndex == minPlayers)
                        {
                            mDatabase.Child("partidas").Child(item.Key).Child("full").SetValueAsync(true);

                        }
                    }
                }

                if (!matchFound)
                {
                    CreateMatch();
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
        mDatabase.Child("partidas").Child(userId).Child("full").SetValueAsync(false).ContinueWithOnMainThread(task =>
        {

            HandleMatch(userId);
        });

       
    }

    public void HandleMatch(string matchId)
    {
        var matchRef = FirebaseDatabase.DefaultInstance
        .GetReference($"partidas/{matchId}");

        matchRef.ValueChanged += UpdateMatch;

    }

    private void UpdateMatch(object sender, ValueChangedEventArgs args)
    {
        playerList.text = "";
        if (args.DatabaseError != null)
        {
            return;
        }

        Dictionary<string, object> partida = (Dictionary<string, object>)args.Snapshot.Value;
        if (partida.ContainsKey("players"))
        {
            Dictionary<string, object> players = (Dictionary<string, object>)partida["players"];

            playerCount.text = players.Keys.Count + "/" + minPlayers + "jugadores";

            for (int i = 0; i < players.Keys.Count; i++)
            {
                int key =i+1;
                playerList.text += players["player"+key.ToString()]+ "\n";
            }
            
        }

        if (partida.ContainsKey("fulll"))
        {
            bool isFull = (bool)partida["full"];
            if (isFull)
            {
                StartCoroutine(ChangeScene());
            }
        }
    }


    IEnumerator ChangeScene()
    {
        fullMatch.gameObject.SetActive(true);
        int time = 10;
        while (time >= 0)
        {
            fullMatch.text = "Empezando partida en" + time;
            yield return new WaitForSeconds(1);
            time -= 1;
        }

        SceneManager.LoadScene("main");

    }
}
