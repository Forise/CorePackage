//Developed by Pavel Kravtsov.
#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Core.EventSystem;

namespace Core.GooglePlayServices
{
    public class GooglePlayLeaderboard : MonoSingleton<GooglePlayLeaderboard>
    {
        #region Fields
        public string id = "CgkI4L656NAFEAIQAw";
        #endregion

        public void PostScore(int score)
        {
            if (PlayGamesPlatform.Instance.localUser.authenticated)
            {
                PlayGamesPlatform.Instance.ReportScore(score, id, (bool success) =>
                {
                    Debug.Log($"Save score to leaderboard is [{success}]");
                });
            }
            else
            {
                Debug.Log("Post Score failed!");
                Debug.Log("Google play Leaderboard [User is not authenticated]");
            }
        }

        public void LoadScores()
        {
            PlayGamesPlatform.Instance.LoadScores(GPGSIds.leaderboard_test_leaderboard, LeaderboardStart.PlayerCentered, 30, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, LoadLeaderboardData_Handler);
        }

        public void LoadSingleScore()
        {
            PlayGamesPlatform.Instance.LoadScores(GPGSIds.leaderboard_test_leaderboard, LoadSingleData_Handler);
        }

        public void LoadBoard(string id)
        {
            ILeaderboard lb = PlayGamesPlatform.Instance.CreateLeaderboard();
            lb.id = id;
            lb.LoadScores(ok =>
            {
                if (ok)
                {
                    LoadUsersAndDisplay(lb);
                }
                else
                {
                    Debug.Log("Error retrieving leaderboard");
                }
            });
        }

        internal void LoadUsersAndDisplay(ILeaderboard lb)
        {
            // get the user ids
            List<string> userIds = new List<string>();

            foreach (IScore score in lb.scores)
            {
                userIds.Add(score.userID);
            }
            // load the profiles and display (or in this case, log)
            Social.LoadUsers(userIds.ToArray(), (users) =>
            {
                string status = "Leaderboard loading: " + lb.title + " count = " +
                    lb.scores.Length;
                foreach (IScore score in lb.scores)
                {
                    IUserProfile user = FindUser(users, score.userID);
                    status += "\n" + score.formattedValue + " by " +
                        (string)((user != null) ? user.userName : "**unk_" + score.userID + "**");
                }
                Debug.Log(status);
            });
        }

        private IUserProfile FindUser(IUserProfile[] users, string id)
        {
            foreach (var user in users)
                if (user.id == id)
                    return user;
            return null;
        }

        public void ShowLeaderboardUI()
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }

        private void LoadLeaderboardData_Handler(LeaderboardScoreData data)
        {
            Debug.Log("LoadLeaderboardData");
            Debug.Log($"Valid: [{data.Valid}]");
            if (data != null)
            {
                if (data.Scores.Length == 0)
                    Debug.Log("Score Lenght == 0");
                foreach (var score in data.Scores)
                {
                    Debug.Log($"Date: [{score.date}]; UserID: [{score.userID}]; Value: [{score.value}]");
                }
            }
            else
                Debug.Log("Load data flom leadeboard [DATA IS NULL]");
        }

        private void LoadSingleData_Handler(IScore[] scores)
        {
            Debug.Log("LoadSingleLeaderboardData");
            if (scores != null)
            {
                if (scores.Length == 0)
                    Debug.Log("Score Lenght == 0");
                foreach (var score in scores)
                {
                    Debug.Log($"Date: [{score.date}]; UserID: [{score.userID}]; Value: [{score.value}]");
                }
            }
            else
                Debug.Log("Load data flom leadeboard [DATA IS NULL]");
        }
    }
}
#endif