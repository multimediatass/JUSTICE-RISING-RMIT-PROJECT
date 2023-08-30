using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tproject.Quiz
{
    public class QuizHandler : MonoBehaviour
    {
        [SerializeField] QuizController quizController;
        public List<QuizController.MQuizContent> mQuizContentList;
        // [SerializeField] bool isMine = false;

        public UnityEvent AfterDialogFinish;

        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.G) && isMine) ShowQuiz();
        }

        public void ShowQuiz()
        {
            quizController.StartQuiz(mQuizContentList, QuizIsDone);
            // isMine = false;
        }

        public void QuizIsDone()
        {
            AfterDialogFinish?.Invoke();
        }
    }
}