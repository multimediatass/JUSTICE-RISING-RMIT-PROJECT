using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Tproject.Quiz
{
    public class QuizController : MonoBehaviour
    {
        public List<MQuizContent> contensStaging;

        [System.Serializable]
        public struct MQuizContent
        {
            public string Question;
            public List<string> Option;
            public string CorrectAnswer;
        }

        [Header("Quiz UI")]
        public GameObject QuizPanel;
        [SerializeField] TextMeshProUGUI questionUI;
        [SerializeField] List<GameObject> optionBtn;

        int _currentIndex = 0;

        // Handler action
        Action quizFinishToHander;

        public void StartQuiz(List<MQuizContent> quizConten, Action act)
        {
            quizFinishToHander = act;

            foreach (var item in quizConten)
            {
                contensStaging.Add(item);
            }

            QuizPanel.SetActive(true);
            SetUpQuestionAndAnswers(_currentIndex);
        }

        private void SetUpQuestionAndAnswers(int targetQuestion)
        {
            questionUI.text = contensStaging[targetQuestion].Question;

            for (int i = 0; i < optionBtn.Count; i++)
            {
                optionBtn[i].GetComponentInChildren<TextMeshProUGUI>().text = contensStaging[targetQuestion].Option[i];
                optionBtn[i].name = contensStaging[targetQuestion].Option[i];
            }
        }

        public void Answer(Button btnAnswerd)
        {
            if (btnAnswerd.name == contensStaging[_currentIndex].CorrectAnswer)
            {
                Debug.Log($"The answer is correct");
            }
            else
            {
                Debug.Log($"Sorry the answer is Worng");
            }

            if (_currentIndex < optionBtn.Count - 1)
            {
                _currentIndex++;
                SetUpQuestionAndAnswers(_currentIndex);
            }
            else
            {
                QuizPanel.SetActive(false);
                contensStaging.Clear();
                quizFinishToHander?.Invoke();
            }
        }
    }
}