using Tproject.Quiz;
using System.Collections.Generic;
using UnityEngine;
using Tproject.VisualNovelV2;

namespace JusticeRising.GameData
{
    [CreateAssetMenu(fileName = "NpcCard", menuName = "GameData/NpcCard")]
    public class NpcCard : ScriptableObject
    {
        public string npcName;
        public string npcRole;
        public List<Sprite> npcImages;

        [Header("Visual Novel Data")]
        public DialogsController.DialogScript DialogScripts;
        public List<string> ConversationSelected;

        [System.Serializable]
        public struct PlayerDialogData
        {
            public string questionSetected;
            public string Answer;
            public string time;
        }

        [Space]
        [Header("Quiz Data")]
        public List<QuizController.MQuizContent> QuizContents;
        public List<PlayerAnswerData> QuizAnswerd = new List<PlayerAnswerData>();

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

        public void AddConversationSelected(List<string> list)
        {
            foreach (var item in list)
            {
                ConversationSelected.Add(item);
            }
        }
    }
}