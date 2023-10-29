
using JusticeRising.GameData;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tproject.Quiz
{
    public class QuizHandler : MonoBehaviour
    {
        public QuizController quizController;
        // public List<QuizController.MQuizContent> mQuizContentList;

        public NpcCard npcCard;
        [SerializeField] bool playOnStart = false;
        public UnityEvent AfterDialogFinish;

        private void Start()
        {
            if (playOnStart) ShowQuiz();
        }

        public void ShowQuiz()
        {
            bool checkquestion = quizController.StartQuiz(npcCard, QuizIsDone);

            if (!checkquestion) AfterDialogFinish?.Invoke();
        }

        public void QuizIsDone()
        {
            AfterDialogFinish?.Invoke();
        }
    }
}