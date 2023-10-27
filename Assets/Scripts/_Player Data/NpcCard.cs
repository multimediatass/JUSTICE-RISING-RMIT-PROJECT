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

        [Space]
        [Header("Visual Novel Data")]
        public List<QuizController.MQuizContent> QuizContents;
        public List<PlayerAnswerData> QuizAnswerd = new List<PlayerAnswerData>();

        [System.Serializable]
        public struct PlayerAnswerData
        {
            public string Question;
            public string PlayerAnswer;

            public PlayerAnswerData(string question, string playerAnswer)
            {
                Question = question;
                PlayerAnswer = playerAnswer;
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