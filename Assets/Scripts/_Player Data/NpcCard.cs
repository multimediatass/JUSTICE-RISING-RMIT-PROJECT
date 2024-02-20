using Tproject.Quiz;
using System.Collections.Generic;
using UnityEngine;
using Tproject.VisualNovelV2;
using System;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using JusticeRising;

namespace JusticeRising.GameData
{
    [CreateAssetMenu(fileName = "NpcCard", menuName = "GameData/NpcCard")]
    public class NpcCard : ScriptableObject
    {
        public string npcName;
        public string npcRole;

        [TextArea]
        public string npcDescription;
        public List<Sprite> npcImages;

        [Header("Visual Novel Data")]
        [SerializeField] private int defaultOpportunity;
        public DialogsController.DialogScript DialogScripts;
        public List<string> ConversationSelected;

        public List<PlayerDialogData> playerDialogData;

        [System.Serializable]
        public struct PlayerDialogData
        {
            public string questionSetected;
            public string Answer;
            public string selectedTimeOnGame;
        }

        [Space]
        [Header("Quiz Data")]
        public List<QuizController.MQuizContent> QuizContents;
        public List<PlayerAnswerData> QuizAnswerd = new List<PlayerAnswerData>();
        public int correctValue;
        public int totalQuestion;
        [System.Serializable]
        public struct PlayerAnswerData
        {
            public string Question;
            public string CorrectAnswer;
            public string PlayerAnswer;
            public string Result;

            public PlayerAnswerData(string question, string correctAnswer, string playerAnswer, string result)
            {
                Question = question;
                CorrectAnswer = correctAnswer;
                PlayerAnswer = playerAnswer;
                Result = result;
            }
        }

        private void Start()
        {
            // if (tempOpportunity == 0)
            //     tempOpportunity = DialogScripts.PlayerOpportunity;
        }

        public void AddConversationSelected(List<string> list)
        {
            if (DialogScripts.PlayerOpportunity < 1) return;

            foreach (var item in list)
            {
                ConversationSelected.Add(item);
            }

            if (DialogScripts.PlayerOpportunity > 0)
                SendVisualNovelDataToPlayfab();
        }

        public void ResetNpcData()
        {
            ConversationSelected.Clear();
            playerDialogData.Clear();
            QuizAnswerd.Clear();
            correctValue = 0;
            totalQuestion = 0;
            DialogScripts.PlayerOpportunity = defaultOpportunity;
        }

        public void SendVisualNovelDataToPlayfab()
        {
            VisualNovelData newData = new VisualNovelData
            {
                PlayerName = $"{GameManager.instance.currentPlayerData.playerProfile.firstName} {GameManager.instance.currentPlayerData.playerProfile.lastName}",
                Username = $"{GameManager.instance.currentPlayerData.playerProfile.username}",
                Email = $"{GameManager.instance.currentPlayerData.playerProfile.email}",
                SelectedNpcName = $"{npcName}",
                NpcRole = $"{npcRole}",
                ConversationSelected = playerDialogData,
                SubmitTimeOnGame = LevelManager.instance.GetCurrentTimeString(),
                Datestamp = DateTime.Now.ToString("dddd, dd MMMM HH:mm")
            };

            string json = JsonConvert.SerializeObject(newData);
            // UpdateUserData($"DialogReport_{DateTime.Now.ToString("yyMMdd_HHmm")}_{npcName}", json);
            UpdateUserData($"DialogReport_{npcName}", json);
        }

        public void SendQuizDataToPlayfab()
        {
            GameManager.instance.IncreaseMasterScore(correctValue * 10);

            QuizData newData = new QuizData
            {
                PlayerName = $"{GameManager.instance.currentPlayerData.playerProfile.firstName} {GameManager.instance.currentPlayerData.playerProfile.lastName}",
                Username = $"{GameManager.instance.currentPlayerData.playerProfile.username}",
                Email = $"{GameManager.instance.currentPlayerData.playerProfile.email}",
                SelectedNpcName = $"{npcName}",
                NpcRole = $"{npcRole}",
                QuestionCount = totalQuestion,
                CorrectValue = correctValue,
                PlayerQuizData = QuizAnswerd,
                SubmitTimeOnGame = LevelManager.instance.GetCurrentTimeString(),
                Datestamp = DateTime.Now.ToString("dddd, dd MMMM HH:mm")
            };

            string json = JsonConvert.SerializeObject(newData);
            // UpdateUserData($"QuizReport_{DateTime.Now.ToString("yyMMdd_HHmm")}_{npcName}", json);
            UpdateUserData($"QuizReport_{npcName}", json);
        }

        void UpdateUserData(string key, string json)
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

    [System.Serializable]
    public class VisualNovelData
    {
        public string PlayerName;
        public string Username;
        public string Email;
        public string SelectedNpcName;
        public string NpcRole;
        public List<NpcCard.PlayerDialogData> ConversationSelected;
        public string SubmitTimeOnGame;
        public string Datestamp;
    }

    [System.Serializable]
    public class QuizData
    {
        public string PlayerName;
        public string Username;
        public string Email;
        public string SelectedNpcName;
        public string NpcRole;
        public int QuestionCount;
        public int CorrectValue;
        public List<NpcCard.PlayerAnswerData> PlayerQuizData;
        public string SubmitTimeOnGame;
        public string Datestamp;
    }
}