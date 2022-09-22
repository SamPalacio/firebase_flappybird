using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using System;
public class ScoreController : MonoBehaviour
{
    DatabaseReference database;
    public GameObject LeaderBoardScreen;
    Coroutine highScoresCoroutine;
    private string m_UserId;
    [SerializeField] TMP_Text scoreText;

    private void Awake()
    {
        database = FirebaseDatabase.DefaultInstance.RootReference;
        m_UserId = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;

    }

    public void WriteNewScore()
    {
        int score = Score.score;
        Debug.Log(score);
        int highScore = PlayerPrefs.GetInt("score");
        if (score > highScore)
        {
            PlayerPrefs.SetInt("score", highScore);
            UserData data = new UserData();
            data.score = score;
            data.userName = PlayerPrefs.GetString("username");
            string json = JsonUtility.ToJson(data);
            database.Child("users").Child(m_UserId).SetRawJsonValueAsync(json);
        }
     
    }
    public void GetUserHighestScores()
    {
        if (highScoresCoroutine==null)
        {
            highScoresCoroutine = StartCoroutine(GetUserHighestScoresData());

        }
    }

    public IEnumerator GetUserHighestScoresData()
    {
       
       
            var task = FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("score").OrderByValue().LimitToLast(10).GetValueAsync();

            yield return new WaitUntil(()=> task.IsCompleted);
         
               if (task.IsFaulted)
               {

               }
               else if (task.IsCompleted)
               {

                   DataSnapshot snapshot = task.Result;
                   scoreText.text = "";
                    if (snapshot != null)
                    {
                        var valueDictionary = (Dictionary<string, object>)snapshot.Value;
                        UserData[] leadersData = new UserData[valueDictionary.Count];

                        int index = 0;
                        foreach (var userDoc in valueDictionary)
                        {

                            var userObject = (Dictionary<string, object>)userDoc.Value;
                            int score = int.Parse(userObject["score"].ToString());
                            string username = userObject["userName"].ToString();
                            Debug.Log(username + "/" + score);
                            leadersData[index] = new UserData(score, username);
                            index++;
                        }
                        Array.Sort(leadersData, (x, y) => y.score.CompareTo(x.score));
                        for (int i = 0; i < index; i++)
                        {
                            scoreText.text += ("\n" + leadersData[i].userName + "  " + leadersData[i].score);

                        }

                        LeaderBoardScreen.SetActive(true);
                    }
                  

               }

        
        highScoresCoroutine = null;

    }
}
[System.Serializable]
public class UserData
{
   public  int score;
   public  string userName;

    public UserData(int score, string userName)
    {
        this.score = score;
        this.userName = userName;
    }
    public UserData()
    {
       
    }
}