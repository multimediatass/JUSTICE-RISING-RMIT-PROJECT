using PlayFab;
using PlayFab.ClientModels;

using System.Collections.Generic;
using UnityEngine;
using JusticeRising.GameData;

namespace JusticeRising
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public PlayerData currentPlayerData;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void AddNpcCard(NpcCard newCard)
        {
            currentPlayerData.npcResumeActivity.Add(newCard);
            // Debug.Log($"AddNPCCard: {newCard.npcName}");
        }

        public void IncreaseMasterScore(int scr)
        {
            currentPlayerData.PlayerScore += scr;

            SendLeaderboardMasterScore();
            GetLeaderBoard();
        }

        public void SendLeaderboardMasterScore()
        {
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate{
                        StatisticName = "MasterScore",
                        Value = currentPlayerData.PlayerScore
                    }

                }
            };

            PlayFabClientAPI.UpdatePlayerStatistics(request,
            (s) => Debug.Log($"Success send to master score"),
            (err) => Debug.LogError($"{err.GenerateErrorReport()}")
            );
        }

        public void GetLeaderBoard()
        {
            var request = new GetLeaderboardRequest
            {
                StatisticName = "MasterScore",
                StartPosition = 0,
                MaxResultsCount = 10
            };

            PlayFabClientAPI.GetLeaderboard(request,
                (result) =>
                {
                    foreach (var item in result.Leaderboard)
                    {
                        Debug.Log($"pos: {item.Position} | name: {item.DisplayName} | score: {item.StatValue}");
                    }
                },
                (err) => Debug.LogError(err.GenerateErrorReport())
            );
        }

        public void UpdateUserData(string key, string json)
        {
            var data = new Dictionary<string, string>
            {
                {key, json}
            };

            var request = new UpdateUserDataRequest
            {
                Data = data
            };

            PlayFabClientAPI.UpdateUserData(request,
                (s) => Debug.Log($"Success send user data to playfab"),
                (err) => Debug.LogError(err.GenerateErrorReport()));
        }
    }
}